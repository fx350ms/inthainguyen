
(function ($) {
    var _orderService = abp.services.app.order,
        _addressService = abp.services.app.customerAddress,
        _waybillService = abp.services.app.waybill,
        _customerService = abp.services.app.customer,
        _$modal = $('#CustomerAddressCreateModal'),
        _$formAddress = _$modal.find('form'),
        _$waybillModal = $('#WaybillCreateModal'),
        _$waybillCreateForm = _$waybillModal.find('form'),
        l = abp.localization.getSource('InTN'),
        _$form = $('#form-create-my-order');
    

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();

        _orderService.edit(order).done(function () {
            _$form[0].reset();
             abp.notify.info(l('SavedSuccessfully'));
             window.location.href = '/Orders';

        }).always(function () {
        });
    });

    _$form.find('cancel-button').on('click', (e) => {

        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();

        _orderService.cancel(order).done(function () {
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            PlayAudio('success', function () {

            });
            window.location.href = '/Orders';
        }).always(function () {
        });
    });
   
     

})(jQuery);
