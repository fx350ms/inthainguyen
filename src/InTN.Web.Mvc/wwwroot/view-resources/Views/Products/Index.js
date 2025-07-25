﻿(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ProductsTable');
    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productService.getProducts,
            inputFilter: function () {
                return $('#ProductSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productsTable.draw(false)
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
                data: 'code',
                sortable: false,
            },
            {
                targets: 3,
                data: 'name',
                sortable: false,
            },
            {
                targets: 4,
                data: 'price',
                className: 'text-right',
                sortable: false,
            },
            {
                targets: 5,
                data: 'cost',
                className: 'text-right',
                sortable: false,
            },
            {
                targets: 6,
                data: 'unit',
                sortable: false,
            },
            {
                targets: 7,
                data: 'description',
                sortable: false,
            },
            {
                targets: 8,
                data: 'invoiceNote',
                sortable: false,
            },
            {
                targets: 9,
                data: 'supplierName',
                sortable: false,
            },
            {
                targets: 10,
                data: 'productTypeName',
                sortable: false,
            },
            {
                targets: 11,
                data: 'productCategoryName',
                sortable: false,
            },
            {
                targets: 12,
                data: 'isActive',
                sortable: false,
                className : 'text-center',
                render: (data) => data
                    ? `<span title="${l('Active')}" style="color:green;"><i class="fas fa-check-circle"></i></span>`
                    : `<span title="${l('Inactive')}" style="color:red;"><i class="fas fa-times-circle"></i></span>`,
            },
            {
                targets: 13,
                data: null,
                sortable: false,
                width: 20,
                render: (data, type, row, meta) => {
                    return [
                        ` <div class="btn-group"> `,
                        `   <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown" aria-expanded="false">`,
                        ` </button>`,
                        ` <div class="dropdown-menu" style="">`,

                        `   <a type="button" class="dropdown-item  bg-primary  btn-edit-product" data-toggle="modal" data-id="${row.id}" data-target="#ProductEditModal">`,
                        `       <i class="fas fa-eye"></i> ${l('Edit')}`,
                        '   </a>',

                        `   <a   href='/Products/EditPriceCombination?id=${row.id}' class="dropdown-item  bg-info  btn-edit-price"  data-id="${row.id}" data-target="#ProductEditPriceModal">`,
                        `       <i class="fas fa-coins"></i> ${l('EditPrice')}`,
                        '   </a>',
                         

                        `   <a type="button" class="dropdown-item  bg-danger  btn-delete-product" data-toggle="modal" data-id="${row.id}"  >`,
                        `       <i class="fas fa-eye"></i> ${l('Delete')}`,
                        '   </a>',
                        `    </div>`,
                        `   </div>`
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
        // Tạo FormData để gửi dữ liệu và tệp
        var formData = new FormData();
        var files = $("input[name='Attachments']")[0].files;

        // Thêm tệp vào FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('Attachments', files[i]);
        }

        // Lấy các dữ liệu từ form và thêm vào FormData
        var data = _$form.serializeFormToObject();
        for (var key in data) {
            formData.append(key, data[key]);
        }

        $.ajax({
            url: abp.appPath + 'api/services/app/Product/CreateWithUploadImage',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData,
            success: function () {
                abp.notify.info(l('SavedSuccessfully'));
                /*delay(1000, () => { window.location.href = '/Orders' });*/

                _$modal.modal('hide');
                _$form[0].reset();
                _$productsTable.ajax.reload();
            },
            error: function () {
                PlaySound('warning'); abp.notify.error(l('SaveFailed'));
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
        //var formData = new FormData(_$form[0]);

        //abp.ui.setBusy(_$modal);
        //_productService.createWithUploadImage(formData).done(function () {
        //    _$modal.modal('hide');
        //    _$form[0].reset();
        //    _$productsTable.ajax.reload();
        //}).always(function () {
        //    abp.ui.clearBusy(_$modal);
        //});
    });

    $(document).on('click', '.btn-edit-product', function () {
        var id = $(this).data('id');
        abp.ajax({
            url: abp.appPath + 'Products/EditModal?Id=' + id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ProductEditModal div.modal-content').html(content);
            }
        });
    });


    $(document).on('click', '.btn-delete-product', function () {
        var id = $(this).data('id');
        abp.message.confirm(
            l('AreYouSureWantToDelete'),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productService.delete({ id: id }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$productsTable.ajax.reload();
                    });
                }
            }
        );
    });

    
    $('.btn-search').on('click', (e) => {
        _$productsTable.ajax.reload();
        return false;
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$productsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);