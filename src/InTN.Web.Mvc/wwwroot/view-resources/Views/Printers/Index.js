(function ($) {
    var _printerService = abp.services.app.printer,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#PrinterCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#PrintersTable');

    var _$printerTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _printerService.getAll,
            inputFilter: function () {
                return $('#PrinterSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$printerTable.draw(false)
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
                width: 80,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 2,
                data: 'name',
                width: 350,
                sortable: true,
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
                width: 150,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-printer" data-id="${row.id}" data-toggle="modal" data-target="#PrinterEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-printer" data-id="${row.id}">${l('Delete')}</button>`
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
        var printer = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _printerService.create(printer).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$printerTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-edit-printer', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'Printers/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#PrinterEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-printer', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _printerService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$printerTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);