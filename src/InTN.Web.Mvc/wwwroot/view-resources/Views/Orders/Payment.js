
(function ($) {
    const _orderService = abp.services.app.order,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#modal-create-transaction'),
        _$form = _$modal.find('form');

    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        // Tạo FormData để gửi dữ liệu và tệp
        var formData = new FormData();
        var files = $("input[name='Attachments']")[0].files;

        // Thêm tệp vào FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('Attachments', files[i]);   
        }

        // Lấy các dữ liệu từ form và thêm vào FormData
        var data = _$form.serializeFormToObject();
        for (var key in data) {
            formData.append(key, data[key]);
        }

        $.ajax({
            url: abp.appPath + 'api/services/app/Order/OrderPayment',
            type: 'PUT',
            processData: false,
            contentType: false,
            data: formData,
            success: function () {
                abp.notify.info(l('SavedSuccessfully'));
                delay(1000, () => { window.location.href = '/Orders' });
            },
            error: function () {
                
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    });

    $('.mask-number').maskNumber({ integer: true, thousands: '.' });
})(jQuery);
