(function ($) {
    var _processGroupService = abp.services.app.processGroup,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProcessGroupCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProcessGroupsTable');

    var _$processGroupsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _processGroupService.getAll,
            inputFilter: function () {
                return $('#ProcessGroupSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$processGroupsTable.draw(false)
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
                width: 100,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 2,
                data: 'name',
                width: 400,
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
                width: 150,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-process-group" data-id="${row.id}" data-toggle="modal" data-target="#ProcessGroupEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-process-group" data-id="${row.id}">${l('Delete')}</button>`
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
        var processGroup = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _processGroupService.create(processGroup).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$processGroupsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-edit-process-group', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'ProcessGroups/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProcessGroupEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-process-group', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _processGroupService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$processGroupsTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);