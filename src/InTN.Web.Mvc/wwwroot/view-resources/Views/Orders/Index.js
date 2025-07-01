(function ($) {

    var _orderService = abp.services.app.order,
        _productPriceCombinations = abp.services.app.productPriceCombinations,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#OrderCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#OrdersTable');

    const orderStatusDescriptions = {
        1: { text: 'Tiếp nhận yêu cầu', color: '#0000FF' }, // Blue
        2: { text: 'Đã báo giá', color: '#800080' }, // Purple
        3: { text: 'Đã xác nhận đơn', color: '#008000' }, // Green
        4: { text: 'Đang thiết kế', color: '#117700' }, // Orange
        5: { text: 'Đang chờ duyệt mẫu', color: '#00FFFF' }, // Cyan
        6: { text: 'Đã duyệt mẫu', color: '#008080' }, // Teal
        7: { text: 'Đã đặt cọc', color: '#117700' }, // Gold
        8: { text: 'Đang in test', color: '#FF69B4' }, // Hot Pink
        9: { text: 'Xác nhận in test (Ok)', color: '#32CD32' }, // Lime Green
        10: { text: 'Đang in', color: '#ff3a3a' }, // Yellow
        11: { text: 'Đang gia công', color: '#009c9f' }, // Pink
        12: { text: 'Đã kiểm tra QC', color: '#7FFF00' }, // Chartreuse
        13: { text: 'Đang giao hàng', color: '#A52A2A' }, // Brown
        14: { text: 'Hoàn thành', color: '#28a745' } // Gray
    };

    const orderPaymentStatusDescriptions = {
        0: { text: 'Chưa thanh toán', color: '#FF0000' }, // Red
        1: { text: 'Đã đặt cọc', color: '#0000FF' }, // Green
        2: { text: 'Đã thanh toán', color: '#008000' }, // Blue
        3: { text: 'Đã chuyển công nợ', color: '#FFA500' } // Orange
    };

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
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return `   <a type="button" data-order-id="${row.id}" href="/Orders/Detail/${row.id}" title="${l('Detail')}" data-toggle="tooltip">` +
                        row.orderCode +
                        '   </a>';
                }
            },
            {
                targets: 3,

                data: 'customerName',
                className: 'text-center',
                render: function (data, type, row, meta) {
                    if (row.customerId == null) {
                        return '<strong>' + row.customerName + '</strong> ';
                    } else {
                        return '<strong>' + row.customerName + '</strong>  <i title="Khách quen" class="far fa-check-circle text-success"></i>';
                    }
                }
            },
            {
                targets: 4,
                data: 'customerPhone',
                className: 'text-center',

            },
            {
                targets: 5,

                className: 'text-center',
                data: 'orderDate',
                render: (data, type, row, meta) => {
                    return formatDateToDDMMYYYYHHmm(data);
                }
            },
            {
                targets: 6,

                data: 'note',
                className: 'text-center',

            },
            {
                targets: 7,
                data: 'totalDeposit',
                className: 'text-right',
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }
            },
            {
                targets: 8,
                data: 'totalAmount',
                className: 'text-right',
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }

            },
            {
                targets: 9,
                width: 120,
                data: 'status',
                className: 'text-center',
                render: (data, type, row, meta) => {

                    // Lấy mô tả và màu sắc của orderStatus từ đối tượng ánh xạ
                    const status = orderStatusDescriptions[row.status];

                    // Trả về mô tả với màu sắc được áp dụng
                    return `<p style="color: ${status ? status.color : 'black'};">${status ? status.text : 'Chưa xác định'}</p>`;

                }
            },
            {
                targets: 10,
                width: 120,
                data: 'paymentStatus',
                className: 'text-center',
                render: (data, type, row, meta) => {

                    // Lấy mô tả và màu sắc của orderStatus từ đối tượng ánh xạ
                    const status = orderPaymentStatusDescriptions[row.paymentStatus];

                    // Trả về mô tả với màu sắc được áp dụng
                    return `<p style="color: ${status ? status.color : 'black'};">${status ? status.text : 'Chưa xác định'}</p>`;

                }
            },
            {
                targets: 11,
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

                        `   <a type="button" class="dropdown-item  bg-primary" data-order-id="${row.id}" href="/Orders/Detail/${row.id}" title="${l('Detail')}" data-toggle="tooltip">`,
                        `       <i class="fas fa-eye"></i> ${l('ViewDetail')}`,
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
                        row.status === 1 ?
                            `   <a type="button" class="dropdown-item bg-success" data-order-id="${row.id}" href="/Orders/CreateQuotation/${row.id}" title="${l('CreateQuotation')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-file-invoice"></i> Tạo phiếu báo giá` +
                            '   </a>' : '',
                        row.status === 2 || row.status === 3 || row.status === 4 ?
                            `   <a type="button" class="dropdown-item bg-success" data-order-id="${row.id}" href="/Orders/CreateDesign/${row.id}" title="${l('CreateDesign')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-palette"></i> Tạo mẫu thiết kế` +
                            '   </a>' : '',
                        row.status === 3 ?
                            `   <a type="button" class="dropdown-item bg-success" data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="${l('CreateDesign')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-tick"></i> Xác nhận mẫu` +
                            '   </a>' : '',

                        row.status === 6 ?
                            `   <a type="button" class="dropdown-item bg-success" data-order-id="${row.id}" href="/Orders/CreateDeposit/${row.id}" data-order-code="${row.orderCode}"  title="Đặt cọc" data-toggle="tooltip">` +
                            `       <i class="fas fa-hand-holding-usd"></i> Đặt cọc` +
                            '   </a>' : '',
                        row.status === 7 ?
                            `   <a type="button" class="dropdown-item bg-success print-test"  data-order-id="${row.id}" data-order-code="${row.orderCode}"   title="In test" data-toggle="tooltip">` +
                            `       <i class="fas fa-paint-roller"></i> In test` +
                            '   </a>' : '',

                        row.status === 8 ?
                            `   <a type="button" class="dropdown-item bg-success confirm-print-test-ok"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="In test OK" data-toggle="tooltip">` +
                            `       <i class="fas fa-thumbs-up"></i> In test OK` +
                            '   </a>' : '',
                        row.status === 9 ?
                            `   <a type="button" class="dropdown-item bg-success start-print"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="Thực hiện in" data-toggle="tooltip">` +
                            `       <i class="fas fa-cogs"></i> Thực hiện in` +
                            '   </a>' : '',
                        row.status === 10 ?
                            `   <a type="button" class="dropdown-item bg-success start-process"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="Hoàn thành in và gia công" data-toggle="tooltip">` +
                            `       <i class="fas fa-truck-pickup"></i> Hoàn thành in và chuyển gia công` +
                            '   </a>' : '',
                        row.status === 11 ?
                            `   <a type="button" class="dropdown-item bg-success delivery-order"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="Hoàn thành gia công và gửi hàng" data-toggle="tooltip">` +
                            `       <i class="fas fa-truck"></i> Hoàn thành gia công & ship` +
                            '   </a>' : '',

                        row.status === 13 ?
                            `   <a type="button" class="dropdown-item bg-success order-completed"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="Hoàn thành đơn" data-toggle="tooltip">` +
                            `       <i class="far fa-check-circle"></i> Hoàn thành` +
                            '   </a>' : '',

                        row.status === 14 && (row.paymentStatus === 0 || row.paymentStatus === 1) ?
                            `   <a type="button" class="dropdown-item bg-success " data-order-id="${row.id}" href="/Orders/Payment/${row.id}" data-order-code="${row.orderCode}"  title="Thanh toán" data-toggle="tooltip">` +
                            `       <i class="far fa-check-circle"></i> Thanh toán` +
                            '   </a>' +
                            `   <a type="button" class="dropdown-item bg-info order-debt"  data-order-id="${row.id}" data-order-code="${row.orderCode}"  title="Chuyển công nợ" data-toggle="tooltip">` +
                            `       <i class="far fa-credit-card"></i> Chuyển công nợ` +
                            '   </a>'

                            : '',
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


    /*Xác nhận design*/
    $(document).on('click', '.approve-design', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToApprove'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.approveDesign({
                        id: orderId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyApprove'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*In test*/
    $(document).on('click', '.print-test', function () {


        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToPrintTest'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.updateStatusToPrintedTest(orderId).done(() => {
                        abp.notify.info(l('StartPrintTest'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*xác nhận in test OK*/
    $(document).on('click', '.confirm-print-test-ok', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToConfirmForPrintTest'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.confirmPrintedTest(orderId).done(() => {
                        abp.notify.info(l('PrintTestOk'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*in thật*/
    $(document).on('click', '.start-print', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToPrint'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.performPrinting(orderId).done(() => {
                        abp.notify.info(l('StartPrint'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*gia công*/
    $(document).on('click', '.start-process', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToProcess'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.performProcessing(orderId).done(() => {
                        abp.notify.info(l('StartPrint'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*Hoàn thành gia công và gửi đơn*/
    $(document).on('click', '.delivery-order', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelivery'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.shipOrder(orderId).done(() => {
                        abp.notify.info(l('Order deliveried'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });

    /*Hoàn thành gia công và gửi đơn*/
    $(document).on('click', '.order-completed', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelivery'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.completeOrder(orderId).done(() => {
                        abp.notify.info(l('StartPrint'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    });


    /*Hoàn thành gia công và gửi đơn*/
    $(document).on('click', '.order-debt', function () {
        var orderId = $(this).attr("data-order-id");
        var orderCode = $(this).attr('data-order-code');

        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToChangeToDebt'),
                orderCode),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.orderDebt(orderId).done(() => {
                        abp.notify.info(l('StartPrint'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
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

    $('.filter-customer-id').select2({
        ajax: {
            delay: 250, // wait 250 milliseconds before triggering the request
            url: abp.appPath + 'api/services/app/Customer/getCustomerListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }

        }

    }).addClass('form-control');

    $('#.select-product-id').on('select2:select', function (e) {
        var data = e.params.data;
        debugger;


    });

})(jQuery);
