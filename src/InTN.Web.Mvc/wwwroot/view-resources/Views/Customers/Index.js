(function ($) {

    var _customerService = abp.services.app.customer,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#CustomerCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#CustomersTable');

    const shippingLines = {
        1: { text: 'Vận chuyển hàng lô', color: 'blue' },
        2: { text: 'Vận chuyển TMĐT', color: 'purple' },
    };
    var _$customersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        select: true,
        listAction: {
            ajaxFunction: _customerService.getAll,
            inputFilter: function () {
                return $('#CustomerSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$customersTable.draw(false)
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
                render: (data, type, row, meta) => {
                 
                   return [
                       `<a type="button" class="edit-customer" data-customer-id="${row.id}" data-toggle="modal" data-target="#CustomerEditModal" title="${l('Edit')}">`,
                       data,
                       `</a>`
                   ].join('');
               }
            },
            

            {
                targets: 3, 
                data: 'phoneNumber',
                sortable: false
            },
            {
                targets: 4,
                data: 'email',
                sortable: false
            },
            {
                targets: 5,
                data: 'address',
                sortable: false
            },
            {
                targets: 6,
                data: 'totalDebt',
                sortable: false,
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }
            }, {
                targets: 7,
                data: 'creditLimit',
                sortable: false,
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                width: 20,
                className : 'text-right',

                defaultContent: '',
                render: (data, type, row, meta) => {

                    return [
                        ` <div class="btn-group"> `,
                        `   <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown" aria-expanded="false">`,
                        `</button>`,
                        ` <div class="dropdown-menu" style="">`,

                        `   <a type="button" class="dropdown-item text-info edit-customer" data-customer-id="${row.id}" data-toggle="modal" data-target="#CustomerEditModal" title="${l('Edit')}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </a>',
                        `   <a type="button" class="dropdown-item text-danger delete-customer" data-customer-id="${row.id}" data-toggle="tooltip"   title="${l('Delete')}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
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
        var customer = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _customerService.create(customer).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$customersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });


    $(document).on('click', '[data-check=all]', function () {
        // Kiểm tra nếu checkbox "select-all" được check hay chưa
        var isChecked = $(this).prop('checked');

        // Cập nhật trạng thái của tất cả các checkbox có data-check="customer"
        $('input[data-check=row]').prop('checked', isChecked);
    });

    $(document).on('change', 'input[data-check=row]', function () {
        // Nếu có checkbox "customer" nào không được check, bỏ check "select-all"
        if ($('input[data-check=row]:not(:checked)').length > 0) {
            $('input[data-check=all]').prop('checked', false);
        } else {
            // Nếu tất cả checkbox "customer" đều được check, thì check "select-all"
            $('input[data-check=all]').prop('checked', true);
        }
    });

    $(document).on('click', '.delete-customer', function () {
        var customerId = $(this).attr("data-customer-id");
        var customerName = $(this).attr('data-customer-name');

        deleteCustomers(customerId, customerName);
    });

   


    function deleteCustomers(customerId, customerName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                customerName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _customerService.delete({
                        id: customerId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$customersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-customer', function (e) {
        var customerId = $(this).attr("data-customer-id");
     
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Customers/EditModal?Id=' + customerId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {

                $('#CustomerEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });


    $(document).on('click', '.assign-to-sale', function (e) {
        // var customerId = $(this).attr("data-customer-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Customers/CustomerLinkToUserModal',
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#CustomerAssignToSaleModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', '.link-to-user', function (e) {
        // var customerId = $(this).attr("data-customer-id");
        var customerId = $(this).attr("data-customer-id");
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Customers/LinkToUser?Id=' + customerId,
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#CustomerLinkToUserModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });


    $(document).on('click', '.reset-password', function (e) {
        // var customerId = $(this).attr("data-customer-id");
        var customerId = $(this).attr("data-customer-id");
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Customers/ResetPassword?Id=' + customerId,
            type: 'GET',
            dataType: 'html',
            success: function (content) {

                $('#CustomerResetPassword div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });
     
    $(document).on('click', 'a[data-target="#CustomersCreateModal"]', (e) => {
        $('.nav-tabs a[href="#customer-details"]').tab('show')
    });

    abp.event.on('customer.edited', (data) => {
        _$customersTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$customersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$customersTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
