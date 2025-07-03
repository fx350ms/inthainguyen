
(function ($) {
    const _orderService = abp.services.app.order,
        _productService = abp.services.app.product,
        _productPriceCombinations = abp.services.app.productPriceCombination,
        _productNotes = abp.services.app.productNotes,

        l = abp.localization.getSource('pbt'),
        _$modal = $('#modal-create-order'),
        _$form = _$modal.find('form');

    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();
        _orderService.createNew(order).done(function () {
            /*_$form[0].reset();*/
            abp.notify.info(l('SavedSuccessfully'));
            delay(1000, () => { window.location.href = '/Orders' });

        }).always(function () {
        });
    });


    $('.select-customer').change(function () {
        const isCasualCustomer = $(this).val() === "1";
        if (isCasualCustomer) {
            $('.select-customer-group').hide();
            $('input[name="CustomerName"], input[name="CustomerPhone"], input[name="CustomerEmail"], textarea[name="CustomerAddress"]').val('');
            $('input[name="TotalDebt"], input[name="CreditLimit"]').val('0'); // Clear TotalDebt and CreditLimit
            $('.debt-group').hide();
        } else {
            $('.debt-group').show();
            $('.select-customer-group').show();
        }
    }).trigger('change'); // Trigger change on page load to set initial state

    // Handle customer selection
    $('.select-customer-id').change(function () {
        var customerId = $(this).val();
        var _customerService = abp.services.app.customer;
        if (customerId) {
            // Simulate fetching customer data (replace with actual AJAX call if needed)
            _customerService.get({ id: customerId }).done(function (customerData) {
                $('input[name="CustomerName"]').val(customerData.name);
                $('input[name="CustomerPhone"]').val(customerData.phoneNumber);
                $('input[name="CustomerEmail"]').val(customerData.email);
                $('input[name="CustomerAddress"]').val(customerData.address);
                $('input[name="TotalDebt"]').val(customerData.totalDebt.toLocaleString()); // Format TotalDebt
                $('input[name="CreditLimit"]').val(customerData.creditLimit?.toLocaleString() || ''); // Format CreditLimit
            });
        }
    });


    $('select[name="CustomerId"').select2({
        ajax: {
            delay: 1500, // wait 1000 milliseconds before triggering the request
            url: abp.appPath + 'api/services/app/Customer/GetCustomerListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }
        }

    }).addClass('form-control');

    $('.select2').select2();

    $('.select-product-id').select2({
        allowClear: false,
        closeOnSelect: false,
        selectOnClose: false,

        ajax: {
            delay: 500,
            url: abp.appPath + 'api/services/app/Product/FilterAndSearchProduct',
            data: function (params) {
                var query = {
                    keyword: params.term,
                    productTypeId: $('select[name="ProductTypeId"]').val(), // Loại sản phẩm
                    productCategoryId: $('select[name="ProductCategoryId"]').val(), // Danh mục sản phẩm
                    supplierId: $('select[name="ProductSupplierId"]').val(), // Nhà cung cấp
                    brandId: $('select[name="BrandId"]').val(), // Thương hiệu
                }
                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'

                return {
                    results: data.result
                };
            }
        }
    })
        .on('select2:select', function (e) {
            var data = e.params.data;

            // Lấy danh sách property tương ứng với sản phẩm
            var productId = data.id;
            _productService.getProductProperties(productId).done(function (properties) {
                var $propertyList = $('.property-list');
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
        });

    function ChangePropertyValue() {
        $(document).on('change', '.product-property', function (a, b, c) {
            // Lấy thông tin sản phẩm và thuộc tính
            const $row = $(this).closest('.order-item');  // Tìm hàng chứa sản phẩm
            GetProductPrice($row);
        });
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
            abp.notify.error('@L("FailedToCalculatePrice")'); // Hiển thị lỗi nếu không tính được giá
        });
    }

    function CalculateTotalPrice($row) {
        const unitPrice = parseFloat($row.find('.unit-price').val().replace('.', '')) || 0; // Lấy giá sản phẩm
        const quantity = parseInt($row.find('input[name="Quantity"]').val()) || 0; // Lấy số lượng sản phẩm

        const totalProductPrice = unitPrice * quantity; // Tính thành tiền
        $row.find('input[name="TotalProductPrice"]').val(formatThousand(totalProductPrice)); // Cập nhật thành tiền vào ô "TotalProductPrice"

        CalculateOrderSummary(); // Cập nhật tổng tiền đơn hàng
    }

    function CalculateOrderSummary() {
        let totalAmount = 0;

        // Duyệt qua tất cả các hàng sản phẩm để tính tổng tiền
        $('#order-detail-list .order-item').each(function () {
            const totalPrice = parseFloat($(this).find('input[name="TotalPrice"]').val().replace(/,/g, '')) || 0;
            totalAmount += totalPrice;
        });

        // Cập nhật tổng tiền vào ô "TotalAmount"
        $('input[name="TotalAmount"]').val(formatThousand(totalAmount));
    }

    $(document).on('input', 'input[name="Quantity"], .unit-price', function () {
        const $row = $(this).closest('.order-item'); // Tìm hàng chứa sản phẩm
        CalculateTotalPrice($row); // Tính thành tiền cho hàng hiện tại
    });

    // Xử lý khi thay đổi thuộc tính sản phẩm
    $(document).on('change', '.product-property', function () {
        const $row = $(this).closest('.order-item'); // Tìm hàng chứa sản phẩm
        GetProductPrice($row); // Tính giá sản phẩm dựa trên thuộc tính
        CalculateTotalPrice($row); // Tính thành tiền cho hàng hiện tại
    });
    // Xử lý xóa item
    $(document).on('click', '.delete-item-button', function () {
        const $orderItem = $(this).closest('.order-item'); // Tìm thẻ div cha có class là order-item
        $orderItem.remove(); // Xóa thẻ div cha
        // Cập nhật hiển thị nút xóa sau khi xóa item
        UpdateDeleteButtonVisibility();
    });

    // Xử lý clone item
    $(document).on('click', '.clone-item-button', function () {
        const $orderItem = $(this).closest('.order-item'); // Tìm thẻ div cha có class là order-item
        const $clonedItem = $orderItem.clone(); // Clone thẻ div cha

        // Reset các giá trị trong item clone
        $clonedItem.find('input').val(''); // Xóa giá trị trong các ô input
        $clonedItem.find('.unit-price').val('0'); // Reset giá trị unit price
        $clonedItem.find('.total-price').val('0'); // Reset giá trị total price
        $clonedItem.find('.quantity').val('1'); // Reset giá trị quantity

        $orderItem.after($clonedItem); // Dán item clone ngay dưới item hiện tại


        // Cập nhật hiển thị nút xóa sau khi clone item
        UpdateDeleteButtonVisibility();
    });

    // Xử lý thêm item mới
    $(document).on('click', '.btn-add-order-item', function () {
        const $newItem = $('#order-detail-list .order-item:first').clone(); // Clone item đầu tiên làm mẫu

        // Reset các giá trị trong item mới
        $newItem.find('input').val(''); // Xóa giá trị trong các ô input
        $newItem.find('.unit-price').val('0'); // Reset giá trị unit price
        $newItem.find('.total-price').val('0'); // Reset giá trị total price
        $newItem.find('.quantity').val('1'); // Reset giá trị quantity

        $('#order-detail-list').append($newItem); // Thêm item mới vào cuối danh sách


        // Cập nhật hiển thị nút xóa sau khi clone item
        UpdateDeleteButtonVisibility();
    });

    // Cải tiến: Tự động tính toán tổng tiền khi thay đổi số lượng hoặc giá sản phẩm
    $(document).on('input', '.quantity, .unit-price', function () {
        const $orderItem = $(this).closest('.order-item'); // Tìm thẻ div cha có class là order-item
        const unitPrice = parseFloat($orderItem.find('.unit-price').val()) || 0; // Lấy giá sản phẩm
        const quantity = parseInt($orderItem.find('.quantity').val()) || 0; // Lấy số lượng sản phẩm

        const totalPrice = unitPrice * quantity; // Tính thành tiền
        $orderItem.find('.total-price').val(totalPrice.toFixed(2)); // Cập nhật giá trị vào ô "TotalPrice"
    });

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
    
    // Cập nhật hiển thị nút xóa khi trang được load
    UpdateDeleteButtonVisibility();

})(jQuery);
