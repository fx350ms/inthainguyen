(function ($) {
    const _orderService = abp.services.app.order,
        _productService = abp.services.app.product,
        _productPriceCombinations = abp.services.app.productPriceCombination,
        _productNotes = abp.services.app.productNote,
        _customerService = abp.services.app.customer,
        _fileUploadService = abp.services.app.fileUpload,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#modal-create-order'),
        _$form = _$modal.find('form');
    Dropzone.autoDiscover = false;
    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }


        var order = _$form.serializeFormToObject();

        order.CreditLimit = order.CreditLimit.replaceAll('.', '');
        order.DeliveryFee = order.DeliveryFee.replaceAll('.', '');
        order.DiscountAmount = order.DiscountAmount.replaceAll('.', '');
        order.Price = order.Price.replaceAll('.', '');
        order.Quantity = order.Quantity.replaceAll('.', '');
        order.TotalAmount = order.TotalAmount.replaceAll('.', '');
        order.TotalCustomerPay = order.TotalCustomerPay.replaceAll('.', '');
        order.TotalDebt = order.TotalDebt.replaceAll('.', '');
        order.TotalDeposit = order.TotalDeposit.replaceAll('.', '');
        order.TotalOrderAmount = order.TotalOrderAmount.replaceAll('.', '');
        order.TotalProductAmount = order.TotalProductAmount.replaceAll('.', '');
        order.TotalProductPrice = order.TotalProductPrice.replaceAll('.', '');
        order.VatAmount = order.VatAmount.replaceAll('.', '');
        order.VatRate = order.VatRate.replaceAll('.', '');
        if (order.NewCustomer) {
            order.CustomerId = 0;
        }
        order.OrderDetails = [];
        /// Add logic here
        // Duyệt qua tất cả các .order-item để chuẩn bị dữ liệu cho OrderDetails
        $('#order-detail-list .order-item').each(function () {
            const $item = $(this);
            const orderItem = {

                ProductId: $item.find('select[name="ProductId"]').val(),
                ProductName: $item.find('select[name="ProductId"] option:selected').text(),
                UnitPrice: parseFloat(
                    ($item.find('input[name="Price"]').val() || '0').replaceAll('.', '')
                ),
                Quantity: parseInt(
                    ($item.find('input[name="Quantity"]').val() || '0').replaceAll('.', '')),
                TotalProductPrice: 0, // Tính ở server
                FileId: $item.find('input[name="FileId"]').val(),
                FileUrl: $item.find('input[name="FileUrl"]').val(),
                FileType: ($item.find('input[name="FileType"]').attr('data-value') == 'url' ? 2 : 1),
                Note: $item.find('input[name="OtherNote"]').val(),
                Properties: [], // Danh sách thuộc tính sản phẩm
                NoteIds: [] // Danh sách ID ghi chú

            };
            
            // Duyệt qua tất cả các thuộc tính sản phẩm
            $item.find('.product-property').each(function () {
                const $property = $(this);
                orderItem.Properties.push({
                    PropertyId: $property.attr('property_id'),
                    PropertyName: $property.attr('property_name'),
                    Value: $property.val()
                });
            });

            // Duyệt qua tất cả các ghi chú sản phẩm
            $item.find('.select-note').each(function () {
                const noteId = $(this).val();
                if (noteId) {
                    orderItem.NoteIds.push(parseInt(noteId));
                }
            });

            order.OrderDetails.push(orderItem); // Thêm chi tiết đơn hàng vào danh sách
        });

        // Gửi dữ liệu đến API
       // abp.ui.setBusy(); // Hiển thị trạng thái loading

        _orderService.createNew(order).done(function () {
            /*_$form[0].reset();*/
            abp.notify.info(l('SavedSuccessfully'));
            delay(1000, () => { window.location.href = '/Orders' });

        }).always(function () {
        });
    });


    $('.select-customer-id').select2({
        ajax: {
            delay: 500, // wait 1000 milliseconds before triggering the request
            url: abp.appPath + 'api/services/app/Customer/GetCustomerListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }
        },
        tags: true
    }).on('select2:select', function (e) {

        var data = e.params.data;
        // Lấy danh sách property tương ứng với sản phẩm
        var customerId = data.id;
        if (customerId && !isNaN(customerId) && customerId != data.text) {
            // Simulate fetching customer data (replace with actual AJAX call if needed)
            _customerService.get({ id: customerId }).done(function (customerData) {
                $('input[name="CustomerName"]').val(customerData.name);
                $('input[name="CustomerPhone"]').val(customerData.phoneNumber);
                $('input[name="CustomerEmail"]').val(customerData.email);
                $('input[name="CustomerAddress"]').val(customerData.address);
                $('input[name="TotalDebt"]').val(customerData.totalDebt.toLocaleString()); // Format TotalDebt
                $('input[name="CreditLimit"]').val(customerData.creditLimit?.toLocaleString() || ''); // Format CreditLimit
                $('input[name="NewCustomer"]').val(false);
            });
        }
        else {
            $('input[name="CustomerName"]').val(data.text);
            $('input[name="NewCustomer"]').val(true);
        }
    });



    /// new update

    function InitializeOrderItem($item) {
        // Khởi tạo lại select2 cho select-product
        $item.find('.select-product-id').select2({
            placeholder: l("SelectProduct"),
            ajax: {
                url: abp.appPath + 'api/services/app/Product/FilterAndSearchProduct',
                data: function (params) {
                    return {
                        keyword: params.term,
                        //productTypeId: $('select[name="ProductTypeId"]').val(),
                        //productCategoryId: $('select[name="ProductCategoryId"]').val(),
                        //supplierId: $('select[name="ProductSupplierId"]').val(),
                        //brandId: $('select[name="BrandId"]').val()
                    };
                },
                processResults: function (data) {
                    return {
                        results: data.result
                    };
                }
            }
        }).on('select2:select', function (e) {
            var data = e.params.data;
            // Lấy danh sách property tương ứng với sản phẩm
            var productId = data.id;
            _productService.getProductProperties(productId).done(function (properties) {
                var $propertyList = $row.find('.property-list');
                $propertyList.empty(); // Clear existing properties
                properties.forEach(function (property) {
                    var $propertyDiv = $('<div>')
                        .addClass('col-md-2 control-item') // Sử dụng col-md-2 để chia cột
                        .append($('<label>').text(property.propertyName)) // Thêm label cho thuộc tính
                        .append(
                            $('<select>')
                                .addClass('form-control product-property ') // Sử dụng select2 và form-control
                                .attr('name', 'Property_' + property.propertyId)
                                .attr('property_name', property.propertyName)
                                .attr('property_id', property.propertyId)
                            // Đặt name theo propertyId
                        );

                    // Thêm các giá trị vào select
                    property.values.forEach(function (option) {
                        var $option = $('<option>')
                            .val(option)
                            .text(option);
                        $propertyDiv.find('select').append($option);
                    });

                    $propertyList.append($propertyDiv); // Thêm thuộc tính vào danh sách
                });
                ChangePropertyValue();
            });
            const $row = $(this).closest('.order-item');  // Tìm hàng chứa sản phẩm

            GetProductPrice($row);
            LoadProductNotes($row, productId); // Tải ghi chú sản phẩm khi chọn sản phẩm)

            InitializeProductNotes($row, productId); // Khởi tạo ghi chú sản phẩm
        });

        // Reset các giá trị trong item
        $item.find('input').val(''); // Xóa giá trị trong các ô input
        $item.find('.unit-price').val('0'); // Reset giá trị unit price
        $item.find('.total-product-price').val('0'); // Reset giá trị total product price
        $item.find('.quantity').val('1'); // Reset giá trị quantity

        // Cập nhật hiển thị nút xóa
        UpdateDeleteButtonVisibility();

        // Xử lý sự kiện thay đổi thuộc tính sản phẩm
        $item.find('.product-property').off('change').on('change', function () {
            const $row = $(this).closest('.order-item'); // Tìm hàng chứa sản phẩm
            GetProductPrice($row); // Tính giá sản phẩm dựa trên thuộc tính
            CalculateTotalPrice($row); // Tính thành tiền cho hàng hiện tại
        });

        // Xử lý sự kiện thay đổi số lượng hoặc giá sản phẩm
        $item.find('input[name="Quantity"], .unit-price').off('input').on('input', function () {
            const $row = $(this).closest('.order-item'); // Tìm hàng chứa sản phẩm
            CalculateTotalPrice($row); // Tính thành tiền cho hàng hiện tại
        });

        // Bổ sung logic tìm button xóa để xóa item
        $item.find('.delete-item-button').off('click').on('click', function () {
            const $orderItem = $(this).closest('.order-item');
            $orderItem.remove();
            UpdateDeleteButtonVisibility();
        });


        $item.find('input[name="FileType"]').off('change').on('change', function () {

            // Get the value of the currently selected radio button
            var selectedFileType = $(this).attr('data-value');

            if (selectedFileType === 'upload') {
                // Hiển thị nhóm upload file, ẩn nhóm nhập URL
                $item.find('.group-upload-file').show();
                $item.find('.group-file-url').hide();

                InitDropzone($item);
                //  InitUploadzone($item);
            } else if (selectedFileType === 'url') {
                // Hiển thị nhóm nhập URL, ẩn nhóm upload file
                $item.find('.group-upload-file').hide();
                $item.find('.group-file-url').show();
            }
        });


    }

    function InitUploadzone($item) {
        $item.find('.file-upload').on('change', function (event) {

            var formData = new FormData();
            var files = this.files;
            if (files.length === 0) {
                console.log('Chưa có file nào được chọn.');
                return; // Dừng lại nếu không có file
            }
            // Thêm tệp vào FormData
            for (var i = 0; i < files.length; i++) {
                formData.append('Files', files[i]);
            }

            $.ajax({
                url: abp.appPath + 'api/services/app/FileUpload/UploadFilesAndGetIds',
                type: 'POST',
                processData: false,
                contentType: false,
                data: formData,
                success: function () {

                },
                error: function () {

                },
                complete: function () {
                    abp.ui.clearBusy(_$form);
                }
            });
        });
    }


    function InitDropzone($item) {

        const dzone = $item.find('.dropzone');

        if (dzone.length === 0) {
            console.error('Không tìm thấy phần tử Dropzone.');
            return;
        }

        const dropzone = new Dropzone(dzone[0], { // Sử dụng dzone[0] để đảm bảo phần tử DOM hợp lệ
            url: abp.appPath + 'api/services/app/FileUpload/UploadFilesAndGetIds', // URL API để xử lý upload file
            //  url: abp.appPath + 'FileUpload/UploadSingleFile', // URL API để xử lý upload file
            paramName: "files",
            method: "post",
            maxFiles: 1, // Giới hạn số lượng file
            maxFilesize: 5, // Giới hạn kích thước file (MB)
            acceptedFiles: ".jpg,.jpeg,.png,.pdf,.doc,.docx", // Các loại file được chấp nhận
            addRemoveLinks: true, // Hiển thị nút xóa file
            dictDefaultMessage: l('DragAndDropFilesHereOrClickToUpload'),
            dictRemoveFile: l('RemoveFile'),
            headers: {
                "X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val(),// Token CSRF nếu cần
                "x-xsrf-token": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (file, response) {
                // Xử lý khi upload thành công
                var fileIds = response.result.join(',')
                $item.find('input[name="FileId"]').val(fileIds); // Lưu ID file vào input hidden
                abp.notify.success(l('FileUploadedSuccessfully'));
            },
            error: function (file, errorMessage) {
                // Xử lý khi upload thất bại
                abp.notify.error(l('FailedToUploadFile'));
            },
            removedfile: function (file) {
                // Xử lý khi xóa file
                $item.find('input[name="FileId"]').val(""); // Xóa giá trị ID file
                abp.notify.info(l('FileRemoved'));
                file.previewElement.remove();
            },

        });

    }


    function InitializeProductNotes($row, productId, level) {
        // Gọi API để lấy danh sách ghi chú cha
        _productNotes.getNotesByProductId(productId, true).done(function (notes) {

            const $noteList = $row.find('.note-list');

            $noteList.find('.note-item').remove();

            if (Array.isArray(notes) && notes.length > 0) {

                level = level || 1;
                const $noteSelect = $('<select>')
                    .addClass('form-control select-note')
                    .attr('name', 'Note_0')
                    .attr('data-level', level)
                    .append($('<option>').val('').text(l('SelectNote')));


                notes.forEach(function (note) {
                    $noteSelect.append($('<option>').val(note.id).text(note.note));
                });

                const $noteDiv = $('<div>')
                    .addClass('control-item col-md-2 note-item') // Hiển thị ghi chú trong cột
                    .append($('<label>').text(l('Note') + ' ' + level)) // Hiển thị tên ghi chú
                    .append($noteSelect);
                $noteList.prepend($noteDiv);

                $($noteSelect).change(function ($this) {
                    LoadChildNotes($noteDiv, $noteSelect, level);
                });
            }

        });
    }

    function LoadChildNotes($parentDiv, $parentNoteSelect, level) {

        const parentId = $parentNoteSelect.val(); // Lấy ID ghi chú cha được chọn

        if (!parentId) {
            return; // Nếu không có parentId, không cần xử lý
        }


        $parentNoteSelect.closest('.note-item').nextAll('.note-item').remove();
        // Gọi API để lấy danh sách ghi chú con
        _productNotes.getNotesByParent(parentId).done(function (notes) {

            if (Array.isArray(notes) && notes.length > 0) {
                const $row = $parentNoteSelect.closest('.order-item'); // Tìm hàng chứa sản phẩm

                // const $noteList = $row.find('.note-list'); // Tìm danh sách ghi chú

                level = level + 1; // Tăng cấp độ ghi chú

                // Tạo select mới cho ghi chú con
                const $childNoteSelect = $('<select>')
                    .addClass('form-control select-note')
                    .attr('name', `Note_${level}`)
                    .attr('data-level', level)
                    .append($('<option>').val('').text(l('SelectNote'))); // Thêm tùy chọn mặc định

                // Thêm các ghi chú con vào select
                notes.forEach(function (note) {
                    $childNoteSelect.append($('<option>').val(note.id).text(note.note));
                });

                // Tạo div chứa ghi chú con
                const $noteDiv = $('<div>')
                    .addClass('control-item col-md-2 note-item') // Hiển thị ghi chú trong cột
                    .append($('<label>').text(l('Note') + ' ' + level)) // Hiển thị tên ghi chú
                    .append($childNoteSelect);

                // Thêm ghi chú con vào danh sách
                $parentDiv.after($noteDiv);


                // Gắn sự kiện thay đổi cho ghi chú con
                $childNoteSelect.off('change').on('change', function () {
                    LoadChildNotes($noteDiv, $(this), level); // Tiếp tục hiển thị ghi chú con tiếp theo
                });
            }
        }).fail(function () {
            abp.notify.error(l('FailedToLoadNotes')); // Hiển thị lỗi nếu không lấy được dữ liệu
        });
    }


    function LoadProductNotes($row, productId) {
        _productNotes.getNotesByProductId(productId, true).done(function (notes) {
            const $select = $row.find('.select-note-1');
            $select.empty(); // Clear existing options
            $select.append($('<option>').val('').text(l('SelectNote'))); // Add default option
            if (Array.isArray(notes) && notes.length > 0) {
                notes.forEach(function (note) {
                    $select.append($('<option>').val(note.id).text(note.note));
                });
            }
            $select.trigger('change'); // Refresh select2 if used
        });
    }

    $('.select-date').daterangepicker(
        {
            "singleDatePicker": true,
            locale: { cancelLabel: 'Clear' }
        }
    );

    $(document).on('input', 'input[name="TotalDeposit"], input[name="DeliveryFee"], input[name="VatRate"], input[name="DiscountAmount"]', function () {
        CalculateOrderSummary(); // Gọi hàm tính toán tổng tiền đơn hàng
    });


    function CalculateOrderSummary() {


        let TotalProductPrice = 0; // Tổng tiền sản phẩm
        let totalDeposit = parseFloat(($('input[name="TotalDeposit"]').val() || '0').replaceAll('.', '')); // Tổng tiền đặt cọc
        let deliveryFee = parseFloat(($('input[name="DeliveryFee"]').val() || '0').replaceAll('.', '')); // Phí vận chuyển
        let vatRate = parseFloat(($('input[name="VatRate"]').val() || '0').replaceAll('.', '')); // % VAT
        let discountAmount = parseFloat(($('input[name="DiscountAmount"]').val() || '0').replaceAll('.', '')); // Tiền giảm giá


        // Duyệt qua tất cả các hàng sản phẩm để tính tổng tiền
        $('#order-detail-list .order-item').each(function () {
            var price = ($(this).find('input[name="TotalProductPrice"]').val() || '0').replaceAll('.', '');
            const totalPrice = parseFloat(price);
            TotalProductPrice += totalPrice;

        });

        // Cập nhật tổng tiền vào ô "TotalProductAmount" của phần tổng kết đơn hàng
        //    $('input[name="TotalProductAmount"]').val(formatThousand(TotalProductPrice));

        // Thêm logic để tính tổng tiền đơn hàng   

        // Tính tiền thuế VAT
        const vatAmount = (TotalProductPrice * vatRate) / 100;

        // Tính tổng tiền đơn hàng
        const totalAmount = TotalProductPrice + deliveryFee + vatAmount - discountAmount;

        // Cập nhật giá trị vào các ô nhập liệu
        $('input[name="TotalProductAmount"]').val(formatThousand(TotalProductPrice)); // Tổng tiền sản phẩm
        $('input[name="VatAmount"]').val(formatThousand(vatAmount)); // Tiền thuế VAT
        $('input[name="TotalAmount"]').val(formatThousand(totalAmount)); // Tổng tiền đơn hàng
        $('input[name="TotalCustomerPay"]').val(formatThousand(totalAmount - totalDeposit)); // Tổng tiền khách phải trả
    }

    function CalculateTotalPrice($row) {
        const unitPrice = parseFloat(($row.find('.unit-price').val() || '0').replaceAll('.', ''));

        const quantity = parseInt($row.find('input[name="Quantity"]').val()) || 0; // Lấy số lượng sản phẩm

        const totalProductPrice = unitPrice * quantity; // Tính thành tiền
        $row.find('input[name="TotalProductPrice"]').val(formatThousand(totalProductPrice)); // Cập nhật thành tiền vào ô "TotalProductPrice"

        CalculateOrderSummary(); // Cập nhật tổng tiền đơn hàng
    }

    function GetProductPrice($row) {

        if ($row.length === 0) {
            console.error('Không tìm thấy hàng chứa sản phẩm (order-item).');
            return;
        }

        // Lấy ID sản phẩm
        const productId = $row.find('.select-product-id').val();
        if (!productId) {
            console.error('Không tìm thấy ID sản phẩm.');
            return;
        }

        const $properties = $row.find('.product-property'); // Tìm tất cả các thuộc tính trong hàng


        const selectedProperties = []; // Lưu các thuộc tính đã chọn

        // Duyệt qua tất cả các thuộc tính của sản phẩm
        $properties.each(function () {
            const propertyName = $(this).attr('name'); // Tên thuộc tính
            const propertyValue = $(this).val(); // Giá trị thuộc tính
            // selectedProperties[propertyName] = propertyValue; // Lưu vào object
            selectedProperties.push({
                propertyId: $(this).attr('property_id'), // Lấy ID thuộc tính
                propertyName: propertyName,
                value: propertyValue
            }); // Lưu vào mảng
        });

        // Gọi API để tính giá sản phẩm dựa trên thuộc tính đã chọn
        _productPriceCombinations.calculatePrice(productId, selectedProperties).done(function (price) {
            // Hiển thị giá sản phẩm trên một đơn vị tính

            $row.find('.unit-price').val(formatThousand(price)); // Cập nhật giá vào ô "Unit Price"
            CalculateTotalPrice($row); // Tính thành tiền cho hàng hiện tại
        }).fail(function () {
            abp.notify.error(l("FailedToCalculatePrice")); // Hiển thị lỗi nếu không tính được giá
        });
    }


    function ChangePropertyValue() {
        $(document).on('change', '.product-property', function (a, b, c) {
            // Lấy thông tin sản phẩm và thuộc tính
            const $row = $(this).closest('.order-item');  // Tìm hàng chứa sản phẩm
            GetProductPrice($row);
        });
    }


    // Hàm kiểm tra số lượng item và hiển thị/ẩn nút xóa
    function UpdateDeleteButtonVisibility() {
        const $orderItems = $('#order-detail-list .order-item'); // Lấy tất cả các item
        const itemCount = $orderItems.length; // Đếm số lượng item

        if (itemCount <= 1) {
            // Nếu chỉ còn 1 item, ẩn nút xóa
            $orderItems.find('.delete-item-button').hide();
        } else {
            // Nếu có từ 2 item trở lên, hiển thị nút xóa
            $orderItems.find('.delete-item-button').show();
        }
    }



    // Xử lý xóa item
    $(document).on('click', '.delete-item-button', function () {
        const $orderItem = $(this).closest('.order-item'); // Tìm thẻ div cha có class là order-item
        $orderItem.remove(); // Xóa thẻ div cha

        // Cập nhật hiển thị nút xóa sau khi xóa item
        UpdateDeleteButtonVisibility();
    });

    // Xử lý clone item
    $(document).on('click', '.clone-item-button', function () {

        // Gọi API để lấy item mẫu
        fetchOrderItemTemplate(function (html) {
            $('#order-detail-list').append(html); // Thêm item mới vào cuối danh sách

            var $newItem = $('#order-detail-list').children().last();

            InitializeOrderItem($newItem);
            UpdateDeleteButtonVisibility();
        });

    });

    // Xử lý thêm item mới
    $(document).on('click', '.btn-add-order-item', function () {
        // Gọi API để lấy item mẫu
        fetchOrderItemTemplate(function (html) {

            $('#order-detail-list').append(html); // Thêm item mới vào cuối danh sách

            var $newItem = $('#order-detail-list').children().last();
            // Gọi hàm InitializeOrderItem để khởi tạo lại các thuộc tính và control cho item mới
            InitializeOrderItem($newItem);

            UpdateDeleteButtonVisibility();
        });

        //const $newItem = $('#order-detail-list .order-item:first').clone(); // Clone item đầu tiên làm mẫu

    });


    // Gọi API để lấy item mẫu
    function fetchOrderItemTemplate(callback) {
        $.ajax({
            url: "/Orders/CreateItemDetail",
            type: "GET",
            dataType: "html",
            success: function (html) {
                if (typeof callback === "function") {
                    callback(html);
                }
            },
            error: function () {
                abp.notify.error(l("FailedToLoadOrderItemTemplate"));
            }
        });
    }

    UpdateDeleteButtonVisibility();

    InitializeOrderItem($('#order-detail-list .order-item:first'));

    $('.mask-number').maskNumber({ integer: true, thousands: '.' });
})(jQuery);
