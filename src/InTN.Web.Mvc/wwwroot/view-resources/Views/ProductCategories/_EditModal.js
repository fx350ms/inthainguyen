(function ($) {
    var _productCategoryService = abp.services.app.productCategory,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProductCategoryEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var productCategory = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _productCategoryService.update(productCategory).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('productCategory.edited', productCategory);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input, select').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);