(function ($) {
    var _productCategoryService = abp.services.app.productCategory,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#ProductCategoryCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductCategoriesTable');

    var _$productCategoriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productCategoryService.getAll,
            inputFilter: function () {
                return $('#ProductCategorySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productCategoriesTable.draw(false)
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
                data: 'parentId',
                sortable: false,
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" class="btn btn-warning btn-edit-product-category" data-id="${row.id}" data-toggle="modal" data-target="#ProductCategoryEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-product-category" data-id="${row.id}">${l('Delete')}</button>`
                    ].join('');
                }
            }
        ]
    });


    abp.event.on('productCategory.edited', (data) => {
        _$productCategoriesTable.ajax.reload();
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var productCategory = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _productCategoryService.create(productCategory).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$productCategoriesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-edit-product-category', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'ProductCategories/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductCategoryEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-product-category', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productCategoryService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$productCategoriesTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);