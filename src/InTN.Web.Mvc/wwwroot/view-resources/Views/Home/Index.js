$(function () {

    'use strict';

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    //-----------------------
    //- MONTHLY SALES CHART -
    //-----------------------


    //---------------------------
    //- END MONTHLY SALES CHART -
    //---------------------------


    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('pbt'),
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
     

    var _$ordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _orderService.getAllOrderToday,
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
                targets: 5,
                data: 'totalAmount',
                className: 'text-right',
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }

            },
            
        ]
    });

     
});
