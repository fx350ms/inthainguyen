(function ($) {
    var _productService = abp.services.app.product,
        _productPriceCombinationService = abp.services.app.productPriceCombination,
        l = abp.localization.getSource('InTN');

    let addedProperties = {}; // { [propertyId]: { name: string, values: [] } }
    $('#property-select').select2({
        placeholder: 'Chọn thuộc tính',
        allowClear: true,
        width: 'resolve',
        closeOnSelect: false,

    }).on('select2:select', function (e, a, b, c) {
        var data = e.params.data;
        const selected = data.id;
        const name = data.text;;
        if (!selected || addedProperties[selected]) return;

        addedProperties[selected] = { id: selected, name: name, values: [] };
        $(`#property-select option[value="${selected}"]`).hide();
        renderPropertyInput(selected, name);
    });

  //  loadProductPriceTable();

    //function loadProductPriceTable() {
    //    const productId = $('#ProductId').val();
    //    if (productId && productId > 0) {

    //        // Gọi API để lấy bảng giá sản phẩm
    //        _productPriceCombinationService.getPriceCombinationsByProductId(productId).done(function (data) {
    //            const $tableBody = $('#product-price-table tbody');
    //            $tableBody.empty(); // Xóa dữ liệu cũ (nếu có)

    //            // Duyệt qua danh sách bảng giá và render vào bảng
    //            data.forEach(function (c) {
    //                debugger;
    //                const combinationItems = c.combinations.map(item => `${item.propertyName}: ${item.value}`).join('<br>');
    //                const row = `
    //                <tr>
    //                    <td>${combinationItems}</td>
    //                    <td>${c.price}</td>
    //                </tr>`;
    //                $tableBody.append(row);
    //            });
    //        }).fail(function () {
    //            abp.notify.error('@L("FailedToLoadProductPrices")');
    //        });
    //    }
    //}

    function renderPropertyInput(propertyId, propertyName) {
        const id = `property-${propertyId}`;
        const html = `
            <div class="col-6 property-item" id="${id}">
                <div class="card card-default">
                    <div class="card-header property-header">
                        <span>${propertyName}</span>
                        <button class="btn btn-danger btn-sm remove-property-btn" data-id="${propertyId}">Xoá</button>
                    </div>
                    <div class="card-body">
                        <div class="form-inline mb-2">
                            <input type="text" class="form-control mr-2 value-input" data-id="${propertyId}" placeholder="Nhập giá trị..." />
                            <button type="button" class="btn btn-info btn-sm add-value-btn" data-id="${propertyId}">Thêm giá trị</button>
                        </div>
                        <div class="value-list" data-id="${propertyId}">
                            <!-- danh sách giá trị -->
                        </div>
                    </div>
                </div>
            </div>`;
        $('#property-values-container').append(html);
    }

    function updateCombinationTable() {
        const props = Object.values(addedProperties);
        if (props.length === 0) {
            $('#combination-table-header').empty();
            $('#combination-table-body').empty();
            return;
        }

        // Tổ hợp Cartesian
        const cartesian = (arrays) => arrays.reduce((a, b) => a.flatMap(d => b.map(e => d.concat(e))), [[]]);

        const combinations = cartesian(props.map(p => p.values.map(v => ({ propId: p.id, propName: p.name, value: v }))));

        // Header
        let headerHtml = '';
        props.forEach(p => headerHtml += `<th>${p.name}</th>`);
        headerHtml += '<th width="200">Giá bán</th>';
        $('#combination-table-header').html(headerHtml);

        // Body
        let bodyHtml = '';
        combinations.forEach((combo, index) => {
            let row = '<tr>';
            combo.forEach(c => row += `<td>${c.value}</td>`);
            row += `<td><input width="200" type="text" value="0" class="form-control price-input mask-number text-right" data-combo='${JSON.stringify(combo)}' /></td>`;
            row += '</tr>';
            bodyHtml += row;
        });

        $('#combination-table-body').html(bodyHtml);
    }

    $('#add-property-btn').click(function () {

        const selected = $('#property-select').val();
        const name = $('#property-select option:selected').data('name');
        if (!selected || addedProperties[selected]) return;

        addedProperties[selected] = { id: selected, name: name, values: [] };
        $(`#property-select option[value="${selected}"]`).hide();
        renderPropertyInput(selected, name);
    });

    $('#property-values-container').on('keypress', '.value-input', function (e) {

        if (e.which === 13) { // Phím Enter

            e.preventDefault();
            const propId = $(this).data('id'); // Lấy ID thuộc tính
            const value = $(this).val().trim(); // Lấy giá trị từ ô nhập
            if (!value || addedProperties[propId].values.includes(value)) return; // Kiểm tra giá trị trùng

            // Thêm giá trị vào danh sách thuộc tính
            addedProperties[propId].values.push(value);
            const tag = `<span class="badge badge-secondary mr-1">${value} <i class="fa fa-times remove-value" style="cursor:pointer" data-id="${propId}" data-value="${value}"></i></span>`;
            $(`.value-list[data-id="${propId}"]`).append(tag);

            // Xóa giá trị trong ô nhập sau khi thêm
            $(this).val('');
            updateCombinationTable(); // Cập nhật bảng tổ hợp
        }
    });

    $('#property-values-container').on('click', '.add-value-btn', function () {
        const propId = $(this).data('id');
        const input = $(this).siblings('.value-input');
        const value = input.val().trim();
        if (!value || addedProperties[propId].values.includes(value)) return;

        addedProperties[propId].values.push(value);
        const tag = `<span class="badge badge-secondary mr-1">${value} <i class="fa fa-times remove-value" style="cursor:pointer" data-id="${propId}" data-value="${value}"></i></span>`;
        $(`.value-list[data-id="${propId}"]`).append(tag);
        input.val('');
        updateCombinationTable();
    });

    $('#property-values-container').on('click', '.remove-value', function () {
        const propId = $(this).data('id');
        const value = $(this).data('value');
        addedProperties[propId].values = addedProperties[propId].values.filter(v => v !== value);
        $(this).parent().remove();
        updateCombinationTable();
    });

    $('#property-values-container').on('click', '.remove-property-btn', function () {
        const propId = $(this).data('id');
        delete addedProperties[propId];
        $(`#property-${propId}`).remove();
        $(`#property-select option[value="${propId}"]`).show();
        updateCombinationTable();
    });

    $('.save-button').click(function () {
        const combinations = [];

        const properties = [];

        // Lấy dữ liệu từ bảng tổ hợp
        $('#combination-table-body tr').each(function () {
            const inputs = $(this).find('.price-input');
            if (inputs.length > 0) {
                const price = parseFloat(inputs.val()) || 0; // Lấy giá trị giá bán
                const combo = JSON.parse(inputs.attr('data-combo')); // Lấy tổ hợp thuộc tính
                combinations.push({
                    Combinations: combo.map(c => ({ PropertyId: c.propId, PropertyName: c.propName, Value: c.value })),
                    Price: price
                });
            }
        });
        // Lấy dữ liệu thuộc tính
        Object.values(addedProperties).forEach(prop => {
            properties.push({
                PropertyId: prop.id,
                PropertyName: prop.name,
                Values: prop.values
            });
        });

        // Tạo object để gửi đến Service
        const data = {
            ProductId: $('#ProductId').val(), // ID sản phẩm
            PriceCombinations: combinations, // Danh sách tổ hợp giá
            Properties: properties // Thuộc tính của sản phẩ
        };

        _productPriceCombinationService.savePriceCombinations(data)
            .done(function () {
                abp.notify.success(l('SavedSuccessfully'));
                // Đóng modal nếu cần
            })
            .fail(function (error) { });
    });

    function Init() {
        const productId = $('#ProductId').val();
        if (productId && productId > 0) {
            _productPriceCombinationService.getPriceCombinationsByProductId(productId).done(function (data) {
                if (!data || data.length === 0) return;

                // Step 1: Khôi phục thuộc tính và giá trị
                data.forEach((item) => {
                    item.combinations.forEach(({ propertyId, propertyName, value }) => {
                        if (!addedProperties[propertyId]) {
                            addedProperties[propertyId] = { id: propertyId, name: propertyName, values: [] };
                            renderPropertyInput(propertyId, propertyName);
                            $(`#property-select option[value="${propertyId}"]`).hide();
                        }

                        if (!addedProperties[propertyId].values.includes(value)) {
                            addedProperties[propertyId].values.push(value);
                            const tag = `<span class="badge badge-secondary mr-1">${value} <i class="fa fa-times remove-value" style="cursor:pointer" data-id="${propertyId}" data-value="${value}"></i></span>`;
                            $(`.value-list[data-id="${propertyId}"]`).append(tag);
                        }
                    });
                });

                // Step 2: Sinh tổ hợp dựa trên thuộc tính đã khôi phục
                updateCombinationTable();

                // Step 3: Gán lại giá đã lưu cho từng tổ hợp
                $('#combination-table-body tr').each(function () {
                    
                    const input = $(this).find('.price-input');
                    const currentCombo = JSON.parse(input.attr('data-combo'));

                    // Tìm tổ hợp tương ứng trong data đã load
                    const found = data.find(d =>
                        d.combinations.length === currentCombo.length &&
                        d.combinations.every(c1 => currentCombo.some(c2 => c1.propertyId == c2.propId && c1.value === c2.value))
                    );

                    if (found) {
                        input.val(formatThousand( found.price));
                    }
                });
            });
            $('.mask-number').maskNumber({ integer: true, thousands: '.' });
        }

       
    }

    Init();
})(jQuery);