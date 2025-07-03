(function ($) {

    var _transactionService = abp.services.app.transaction,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#TransactionCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#TransactionsTable');

    
    var _$transactionsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        select: true,
        listAction: {
            ajaxFunction: _transactionService.getAll,
            inputFilter: function () {
                return $('#TransactionSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$transactionsTable.draw(false)
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
                width: 30,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },

            {
                targets: 2,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return `   <a type="button" data-order-id="${row.id}" href="/Transaction/Detail/${row.id}" title="${l('Detail')}" data-toggle="tooltip">` +
                        row.transactionCode +
                        '   </a>';
                }
            },
            {
                targets: 3,
                className: 'text-center',
                data: 'customerName'
            },
            {
                targets: 4,
                data: 'amount',
                className: 'text-right',
                render: function (data, type, row, meta) {
                    return formatThousand(data);
                }
            },
            {
                targets: 5,
                className: 'text-center',
                data: 'description'
            },
            {
                targets: 6,
                className: 'text-center',
                data: 'creationTime',
                render: (data, type, row, meta) => {
                    return formatDateToDDMMYYYYHHmm(data);
                }
            },
            
            {
                targets: 7,
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

                        `   <a type="button" class="dropdown-item text-info edit-transaction" data-transaction-id="${row.id}" data-toggle="modal" data-target="#TransactionEditModal" title="${l('Edit')}">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </a>',
                        `   <a type="button" class="dropdown-item text-danger delete-transaction" data-transaction-id="${row.id}" data-toggle="tooltip"   title="${l('Delete')}">`,
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
        var transaction = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _transactionService.create(transaction).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$transactionsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });


    $(document).on('click', '[data-check=all]', function () {
        // Kiểm tra nếu checkbox "select-all" được check hay chưa
        var isChecked = $(this).prop('checked');

        // Cập nhật trạng thái của tất cả các checkbox có data-check="transaction"
        $('input[data-check=row]').prop('checked', isChecked);
    });

    $(document).on('change', 'input[data-check=row]', function () {
        // Nếu có checkbox "transaction" nào không được check, bỏ check "select-all"
        if ($('input[data-check=row]:not(:checked)').length > 0) {
            $('input[data-check=all]').prop('checked', false);
        } else {
            // Nếu tất cả checkbox "transaction" đều được check, thì check "select-all"
            $('input[data-check=all]').prop('checked', true);
        }
    });

    $(document).on('click', '.delete-transaction', function () {
        var transactionId = $(this).attr("data-transaction-id");
        var transactionName = $(this).attr('data-transaction-name');

        deleteTransactions(transactionId, transactionName);
    });

   


    function deleteTransactions(transactionId, transactionName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                transactionName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _transactionService.delete({
                        id: transactionId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$transactionsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-transaction', function (e) {
        var transactionId = $(this).attr("data-transaction-id");
     
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Transactions/EditModal?Id=' + transactionId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {

                $('#TransactionEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

     
     
    $(document).on('click', 'a[data-target="#TransactionsCreateModal"]', (e) => {
        $('.nav-tabs a[href="#transaction-details"]').tab('show')
    });

    abp.event.on('transaction.edited', (data) => {
        _$transactionsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

     
    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$transactionsTable.ajax.reload();
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
})(jQuery);
