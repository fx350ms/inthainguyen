(function ($) {
    var _productTypeService = abp.services.app.productType,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductTypeCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductTypesTable');

    var _$productTypesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productTypeService.getAll,
            inputFilter: function () {
                return $('#ProductTypeSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productTypesTable.draw(false)
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
                data: 'description',
                sortable: false,
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-product-type" data-id="${row.id}" data-toggle="modal" data-target="#ProductTypeEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-product-type" data-id="${row.id}">${l('Delete')}</button>`
                    ].join('');
                }
            }
        ]
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var productType = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _productTypeService.create(productType).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$productTypesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });


    abp.event.on('productType.edited', (data) => {
        _$productTypesTable.ajax.reload();
    });

    $(document).on('click', '.btn-edit-product-type', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'ProductTypes/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductTypeEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-product-type', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productTypeService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$productTypesTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);