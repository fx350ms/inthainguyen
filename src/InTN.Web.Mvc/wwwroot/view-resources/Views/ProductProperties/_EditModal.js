(function ($) {
    var _productPropertyService = abp.services.app.productProperty,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductPropertyEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var productProperty = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _productPropertyService.update(productProperty).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('productProperty.edited', productProperty);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);