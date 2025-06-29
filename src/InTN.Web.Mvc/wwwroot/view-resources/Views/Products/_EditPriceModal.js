﻿$(function () {
    let addedProperties = {};
    
    $('#property-select').select2({
        placeholder: 'Chọn thuộc tính',
        allowClear: true,
        width: 'resolve'
    });

    function renderPropertyInput(propertyId, propertyName) {
        const id = `property-${propertyId}`;
        const html = `
            <div class="card card-default mb-2" id="${id}">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>${propertyName}</span>
                    <button class="btn btn-danger btn-sm remove-property-btn" data-id="${propertyId}">Xoá</button>
                </div>
                <div class="card-body">
                    <div class="form-inline mb-2">
                        <input type="text" class="form-control mr-2 value-input" placeholder="Nhập giá trị..." />
                        <button type="button" class="btn btn-info btn-sm add-value-btn" data-id="${propertyId}">Thêm giá trị</button>
                    </div>
                    <div class="value-list" data-id="${propertyId}"></div>
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

        const cartesian = (arrays) => arrays.reduce((a, b) => a.flatMap(d => b.map(e => d.concat(e))), [[]]);

        const combinations = cartesian(props.map(p => p.values.map(v => ({ propId: p.id, propName: p.name, value: v }))));

        // Header
        let headerHtml = '';
        props.forEach(p => headerHtml += `<th>${p.name}</th>`);
        headerHtml += '<th>Giá bán</th>';
        $('#combination-table-header').html(headerHtml);

        // Body
        let bodyHtml = '';
        combinations.forEach((combo, index) => {
            let row = '<tr>';
            combo.forEach(c => row += `<td>${c.value}</td>`);
            row += `<td><input type="number" class="form-control price-input" data-combo='${JSON.stringify(combo)}' /></td>`;
            row += '</tr>';
            bodyHtml += row;
        });

        $('#combination-table-body').html(bodyHtml);
    }
    $(document).on('click', '#add-property-btn', function () {
        const selected = $('#property-select').val();
        const name = $('#property-select option:selected').text();
        if (!selected || addedProperties[selected]) return;

        addedProperties[selected] = { id: selected, name: name, values: [] };
        $(`#property-select option[value="${selected}"]`).prop('disabled', true);
        $('#property-select').val(null).trigger('change');
        renderPropertyInput(selected, name);
    });

    // Dùng delegation cho các nút sinh động
    $(document).on('click', '.add-value-btn', function () {
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

    $(document).on('click', '.remove-value', function () {
        const propId = $(this).data('id');
        const value = $(this).data('value');
        addedProperties[propId].values = addedProperties[propId].values.filter(v => v !== value);
        $(this).parent().remove();
        updateCombinationTable();
    });

    $(document).on('click', '.remove-property-btn', function () {
        const propId = $(this).data('id');
        delete addedProperties[propId];
        $(`#property-${propId}`).remove();
        $(`#property-select option[value="${propId}"]`).prop('disabled', false);
        updateCombinationTable();
    });

    $('#save-combination-btn').on('click', function () {
        const result = [];

        $('#combination-table-body tr').each(function () {
            const inputs = $(this).find('.price-input');
            if (inputs.length > 0) {
                const price = parseFloat(inputs.val()) || 0;
                const combo = JSON.parse(inputs.attr('data-combo'));
                result.push({
                    Combination: combo.map(c => ({ PropertyId: c.propId, PropertyName: c.propName, Value: c.value })),
                    Price: price
                });
            }
        });

        $('#PriceCombination').val(JSON.stringify(result));
        abp.message.success('Dữ liệu đã được lưu vào JSON, bạn có thể gửi lên server.');
    });
});
