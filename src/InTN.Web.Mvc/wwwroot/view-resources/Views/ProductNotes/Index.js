(function ($) {
    var _productNoteService = abp.services.app.productNote,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductNoteCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductNotesTable');


    var _$productNotesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productNoteService.getData,
            inputFilter: function () {
                return $('#ProductNoteSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productNotesTable.draw(false)
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
                data: 'note',
                sortable: false,
            },
            {
                targets: 2,
                sortable: false,
                data: 'parentId',
            },
            {
                targets: 3,
                sortable: false,
                data: 'productCategoryName',
            },

            {
                targets: 4,
                data: null,
                sortable: false,
                width: 20,
                className: 'text-right',
                defaultContent: '',
                render: (data, type, row, meta) => {
                    const isEditable = row.status === 1; // Only allow edit/delete if status is 1

                    return [
                        ` <div class="btn-group"> `,
                        `   <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown" aria-expanded="false">`,
                        ` </button>`,
                        ` <div class="dropdown-menu" style="">`,
                        `   <a type="button" class="dropdown-item  bg-primary btn-edit-note" data-order-id="${row.id}"  data-toggle="modal" data-target="#ProductNoteEditModal" title="${l('Detail')}" data-toggle="tooltip">`,
                        `       <i class="fas fa-eye"></i> ${l('ViewDetail')}`,
                        '   </a>',
                        `   <a type="button" class="dropdown-item  bg-primary btn-delete-note" data-order-id="${row.id}"  data-toggle="modal"   title="${l('Delete')}" data-toggle="tooltip">`,
                        `       <i class="fas fa-eye"></i> ${l('Delete')}`,
                        '   </a>',

                        `    </div>`,
                        `   </div>`
                    ].join('');

                }
            }
 
        ]
    });

    $(document).on('click', '.btn-edit-note', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'ProductNotes/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductNoteEditModal div.modal-content').html(content);
            }
        });
    });
    $('.btn-search').on('click', (e) => {
        _$productNotesTable.ajax.reload();
        return false;
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$productNotesTable.ajax.reload();
            return false;
        }
    });

    $(document).on('click', '.btn-delete-note', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productNoteService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$productNotesTable.ajax.reload();
                    });
                }
            }
        );
    });

    _$form.find('.save-button').on('click', (e) => {
    
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var note = _$form.serializeFormToObject();
        abp.ui.setBusy(_$modal);
        _productNoteService.create(note).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
           
            _$productNotesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });


        return false;
    });

     
    $('.select-category-id').select2().on('select2:select', function (e) {
        
        var categoryId = e.params.data.id;
        $('.select-parent-note').empty() .append(
            $('<option>', {
                value : '',
                text: l('SelectParentNote')
            })
        );
        _productNoteService.getNotesByProductCategoryId(categoryId).done(function (data) {
            if (data && Array.isArray(data)) {
                data.forEach(function (note) {
                    $('.select-parent-note').append(
                        $('<option>', {
                            value: note.id,
                            text: note.note
                        })
                    );
                });
            }
            // Optionally, trigger change event if needed
            $('.select-parent-note').trigger('change');
        });
         
    });
     

})(jQuery);