(function ($) {
    var _brandService = abp.services.app.brand,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#BrandCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#BrandsTable');

    var _$brandsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _brandService.getAll,
            inputFilter: function () {
                return $('#BrandSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$brandsTable.draw(false)
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
                width : 100,
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
                        `<button type="button" class="btn btn-warning btn-edit-brand" data-id="${row.id}" data-toggle="modal" data-target="#BrandEditModal">${l('Edit')}</button>`,
                        `<button type="button" class="btn btn-danger btn-delete-brand" data-id="${row.id}">${l('Delete')}</button>`
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
        var brand = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _brandService.create(brand).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$brandsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });
    abp.event.on('brand.edited', (data) => {
        _$brandsTable.ajax.reload();
    });

  
    $(document).on('click', '.btn-edit-brand', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'Brands/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#BrandEditModal div.modal-content').html(content);
            }
        });
    });

    $(document).on('click', '.btn-delete-brand', function () {
        var id = $(this).attr("data-order-id");
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _brandService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$brandsTable.ajax.reload();
                    });
                }
            }
        );
    });
})(jQuery);