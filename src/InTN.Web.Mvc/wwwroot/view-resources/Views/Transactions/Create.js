(function ($) {
    const _transactionService = abp.services.app.transaction,
        _orderService = abp.services.app.order,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#modal-create-transaction'),
        _$form = _$modal.find('form');

    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }


        // Tạo FormData để gửi dữ liệu và tệp
        var formData = new FormData();
        var files = $("input[name='FileContent']")[0].files;

        // Thêm tệp vào FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('Attachments', files[i]);
        }

        // Lấy các dữ liệu từ form và thêm vào FormData
        var data = _$form.serializeFormToObject();
        
        // Lấy CustomerId và CustomerName từ dropdown
        var customerId = $('select[name="CustomerId"]').val();

        if (customerId) {
            data.CustomerId = customerId;
            data.CustomerName = $('select[name="CustomerId"] option:selected').text();
        }
        for (var key in data) {
            formData.append(key, data[key]);
        }

        abp.ui.setBusy(_$modal);
        $.ajax({
            url: '/api/services/app/Transaction/CreateWithAttachment',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                abp.notify.info(l('SavedSuccessfully'));
                delay(1000, () => { window.location.href = '/Transaction' });
            },
            error: function (xhr) {
                abp.message.error(l('ErrorOccurred'));
                console.error(xhr);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    });
     

    $('.select-customer-id').select2({
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


    $('.mask-num').maskNumber({ integer: true, thousands: '.' });


    $('.select-customer').change(function () {
        const isCasualCustomer = $(this).val() === "1";
        if (isCasualCustomer) {
            $('.select-customer-group').hide();
            $('input[name="TotalDebt"], input[name="CreditLimit"]').val('0'); // Clear TotalDebt and CreditLimit
            $('.debt-group').hide();
        } else {
            $('.debt-group').show();
            $('.select-customer-group').show();
        }
    }).trigger('change'); // Trigger change on page load to set initial state

    // Handle customer selection
    $('.select-customer-id').change(function () {
        var customerId = $(this).val();

        var _customerService = abp.services.app.customer;

        if (customerId) {
            // Simulate fetching customer data (replace with actual AJAX call if needed)
            _customerService.get({ id: customerId }).done(function (customerData) {
                debugger;
                $('input[name="TotalDebt"]').val(customerData.totalDebt.toLocaleString()); // Format TotalDebt
                $('input[name="CreditLimit"]').val(customerData.creditLimit?.toLocaleString() || ''); // Format CreditLimit
            });

        }
    });

    
    // Xử lý khi chọn đơn hàng
    $('.select-order-id').change(function () {
        var orderId = $(this).val();

        if (orderId) {
            _orderService.get({ id: orderId }).done(function (orderData) {
                $('input[name="order_TotalDeposit"]').val(orderData.totalDeposit.toLocaleString()); // Số tiền đã cọc
                $('input[name="order_TotalAmount"]').val(orderData.totalAmount.toLocaleString()); // Tổng số tiền
                const amountToPay = orderData.totalAmount - orderData.totalDeposit; // Số tiền cần thanh toán
                $('input[name="Amount"]').val(amountToPay.toLocaleString()); // Điền vào input Amount
            });
        }
    });

    $('select[name="OrderId"]').select2({
        ajax: {
            delay: 250,
            url: abp.appPath + 'api/services/app/Order/getOrderListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }
        }
    }).addClass('form-control');
    
    $('.select-trans-type').change(function () {
        const transactionType = $(this).val();
      
        if (transactionType === "3") { // Thanh toán công nợ
            $('.order-group').hide();
            $('.order-debt-group').hide();
            $('.customer-group').show();
            $('.debt-group').show();
            $('.select-order-id').val('');

        } else if (transactionType === "2") { // Thanh toán đơn hàng
            $('.order-group').show();
            $('.order-debt-group').show();
            $('.customer-group').hide();
            $('.debt-group').hide();
            $('.select-customer-id').val('');
        }
    }).trigger('change');
})(jQuery);