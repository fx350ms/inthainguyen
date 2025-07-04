(function ($) {
    var _productPropertyService = abp.services.app.productProperty,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductPropertyCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductPropertiesTable');

    var _$productPropertiesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productPropertyService.getAll,
            inputFilter: function () {
                return $('#ProductPropertySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productPropertiesTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 2,
                data: 'name',
                sortable: false,
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-product-property" data-id="${row.id}" data-toggle="modal" data-target="#ProductPropertyEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-product-property" data-id="${row.id}">${l('Delete')}</button>`
                    ].join('');
                }
            }
        ]
    });

    abp.event.on('productProperty.edited', (data) => {
        _$productPropertiesTable.ajax.reload();
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var productProperty = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _productPropertyService.create(productProperty).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$productPropertiesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-edit-product-property', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'ProductProperties/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductPropertyEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-product-property', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productPropertyService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$productPropertiesTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);