(function ($) {
    var _supplierService = abp.services.app.supplier,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#SupplierCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#SuppliersTable');

    var _$suppliersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _supplierService.getAll,
            inputFilter: function () {
                return $('#SupplierSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$suppliersTable.draw(false)
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
                data: 'address',
                sortable: false,
            },
            {
                targets: 4,
                data: 'phone',
                sortable: false,
            },
            {
                targets: 5,
                data: 'email',
                sortable: false,
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-supplier" data-id="${row.id}" data-toggle="modal" data-target="#SupplierEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-supplier" data-id="${row.id}">${l('Delete')}</button>`
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
        var supplier = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _supplierService.create(supplier).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$suppliersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    abp.event.on('supplier.edited', (data) => {
        _$suppliersTable.ajax.reload();
    });

    $(document).on('click', '.btn-edit-supplier', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'Suppliers/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#SupplierEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-supplier', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _supplierService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$suppliersTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);