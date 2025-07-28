(function ($) {
    var _processService = abp.services.app.process,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProcessCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#process-table');

    var _$processTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _processService.getAll,
            inputFilter: function () {
                return $('#ProcessSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$processTable.draw(false)
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
                data: 'name',
                width: 200,
                sortable: true,
            },
            {
                targets: 1,
                data: 'description',
                width: 400,
                sortable: false,
            },
            {
                targets: 2,
                data: 'status',
                width: 100,
                sortable: true,
                render: (data) => {
                    return data === 1 ? l('Active') : l('Inactive');
                }
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                width: 150,
                render: (data, type, row, meta) => {
                    return [
                        /*   `<button type="button" class="btn btn-success btn-config-process" data-id="${row.id}" data-toggle="modal" data-target="#ProcessEditModal">${l('Config')}</button>`,*/
                        `<a type="button" class="btn btn-success btn-edit-process" href='/Processes/Config/${row.id}' data-id="${row.id}"  >${l('Config')}</a>`,
                        `<button type="button" class="btn btn-warning btn-edit-process" data-id="${row.id}" data-toggle="modal" data-target="#ProcessEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-process" data-id="${row.id}">${l('Delete')}</button>`
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
        var process = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _processService.create(process).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$processTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-edit-process', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'Process/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProcessEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-process', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _processService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$processTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);