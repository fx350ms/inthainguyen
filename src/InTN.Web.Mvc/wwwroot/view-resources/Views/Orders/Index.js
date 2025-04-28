(function ($) {

    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#OrderCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#OrdersTable');
    const orderStatusDescriptions = {
        1: { text: 'Đã ký gửi', color: 'blue' },
        2: { text: 'Hàng về kho TQ', color: 'purple' },
        3: { text: 'Đang vận chuyển quốc tế', color: 'orange' },
        4: { text: 'Đã đến kho VN', color: 'green' },
        5: { text: 'Đang giao đến khách', color: 'cyan' },
        6: { text: 'Đã giao', color: 'gray' },
        7: { text: 'Khiếu nại', color: 'red' },
        8: { text: 'Hoàn tiền', color: 'pink' },
        9: { text: 'Huỷ', color: 'darkgray' },
        10: { text: 'Hoàn thành đơn', color: 'teal' }
    };
    debugger;
    var _$ordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,

        listAction: {
            ajaxFunction: _orderService.getAll,
            inputFilter: function () {
                return $('#OrderSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$ordersTable.draw(false)
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
                width: 20,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                targets: 2,
                width: 20,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return '';
                }
            },

             
            {
                targets: 3,
                data: null,
                sortable: false,
                width: 20,
                className : 'text-right',
                defaultContent: '',
                render: (data, type, row, meta) => {
                    const isEditable = row.orderStatus === 1; // Only allow edit/delete if status is 1
                    const canCreatePackage = row.orderStatus === 1 || row.orderStatus === 2; // Allow creating package if status is 1 or 2
                    const canCreateDeliveryRequest = row.orderStatus === 4; // Allow creating delivery request if status is 4
                    const canMakePayment = row.paymentStatus === 1; // Allow making payment if payment status is 'Chờ thanh toán'


                    return [
                        ` <div class="btn-group"> `,
                        `   <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown" aria-expanded="false">`,
                        ` </button>`,
                        ` <div class="dropdown-menu" style="">`,
                      
                        `   <a type="button" class="dropdown-item  bg-primary" data-order-id="${row.id}" href="/Orders/Detail/${row.id}" title="${l('Detail')}" data-toggle="tooltip">`,
                        `       <i class="fas fa-eye"></i> ${l('View')}`,
                        '   </a>',
                        isEditable ?
                            `   <a type="button" class="dropdown-item bg-secondary" data-order-id="${row.id}" href="/Orders/Edit/${row.id}" title="${l('Edit')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-pencil-alt"></i>${l('Edit')}` +
                            '   </a>' :
                            '',
                        isEditable ?
                            `   <button type="button" class="dropdown-item bg-danger delete-order" data-order-id="${row.id}" data-order-name="Đơn hàng: ${row.orderNumber}, mã vận đơn ${row.waybillNumber}" title="${l('Delete')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-trash"></i> ${l('Delete')}` +
                            '   </button>' :
                            '',
                        row.orderStatus >= 2 ?
                            `   <a type="button" class="dropdown-item bg-info view-package-list" data-order-id="${row.id}"   title="${l('ViewPackageList')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-box"></i> ${l('ViewPackageList')}` +
                            '   </a>' :
                            '',
                        `    </div>`,
                        `   </div>`
                    ].join('');


                   
                }
            }
        ]
    });

    $('#input-date-range').daterangepicker(
        {
            locale: { cancelLabel: 'Clear' }
        }
    ).val('');
    $('#input-date-range').on('apply.daterangepicker', function (ev, picker) {
        $('#start-date').val(picker.startDate.format('DD-MM-YYYY'));
        $('#end-date').val(picker.endDate.format('DD-MM-YYYY'));
    });

    _$form.find('.save-button').on('click', (e) => {

        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _orderService.create(order).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            PlayAudio('success', function () {
               
            });
            _$ordersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-order', function () {
        var orderId = $(this).attr("data-order-id");
        var orderName = $(this).attr('data-order-name');

        deleteOrders(orderId, orderName);
    });

    function deleteOrders(orderId, orderName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                orderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.delete({
                        id: orderId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-order', function (e) {
        var orderId = $(this).attr("data-order-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Orders/EditModal?Id=' + orderId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {

                $('#OrderEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });
 

    //$(document).on('click', 'a[data-target="#OrdersCreateModal"]', (e) => {
    //    $('.nav-tabs a[href="#order-details"]').tab('show')
    //});

    abp.event.on('order.edited', (data) => {
        _$ordersTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$ordersTable.ajax.reload();
        return false;
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$ordersTable.ajax.reload();
            return false;
        }
    });
     

})(jQuery);
