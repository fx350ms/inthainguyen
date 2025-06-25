
(function ($) {
    const _orderService = abp.services.app.order,
        _productService = abp.services.app.product,
        l = abp.localization.getSource('pbt'),
        _$modal = $('#modal-create-order'),
        _$form = _$modal.find('form');

    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();
        _orderService.createNew(order).done(function () {
            /*_$form[0].reset();*/
            abp.notify.info(l('SavedSuccessfully'));
            delay(1000, () => { window.location.href = '/Orders' });
            
        }).always(function () {
        });
    });


    $('.select-customer').change(function () {
        const isCasualCustomer = $(this).val() === "1";
        if (isCasualCustomer) {
            $('.select-customer-group').hide();
            $('input[name="CustomerName"], input[name="CustomerPhone"], input[name="CustomerEmail"], textarea[name="CustomerAddress"]').val('');
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
                $('input[name="CustomerName"]').val(customerData.name);
                $('input[name="CustomerPhone"]').val(customerData.phoneNumber);
                $('input[name="CustomerEmail"]').val(customerData.email);
                $('input[name="CustomerAddress"]').val(customerData.address);
                $('input[name="TotalDebt"]').val(customerData.totalDebt.toLocaleString()); // Format TotalDebt
                $('input[name="CreditLimit"]').val(customerData.creditLimit?.toLocaleString() || ''); // Format CreditLimit
            });
        }
    });



    $('select[name="CustomerId"').select2({
        ajax: {
            delay: 1000, // wait 1000 milliseconds before triggering the request
            url: abp.appPath + 'api/services/app/Customer/GetCustomerListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }
        }

    }).addClass('form-control');

    $('.select2').select2();

   

    $('.select-product-id').select2({
        ajax: {
            url: abp.appPath + 'api/services/app/Product/FilterAndSearchProduct', 
            data: function (params) {
                var query = {
                    keyword: params.term,
                    productTypeId: $('select[name="ProductTypeId"]').val(), // Loại sản phẩm
                    productCategoryId: $('select[name="ProductCategoryId"]').val(), // Danh mục sản phẩm
                    supplierId: $('select[name="ProductSupplierId"]').val(), // Nhà cung cấp
                    brandId: $('select[name="BrandId"]').val(), // Thương hiệu
                }
                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                
                return {
                    results: data.result
                };
            }
        }
    });

})(jQuery);
