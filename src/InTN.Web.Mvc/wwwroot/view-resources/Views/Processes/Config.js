(function ($) {
    var _processService = abp.services.app.process,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#ProcessCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#process-table');
 
    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$form.valid()) {
            return;
        }
        var process = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _processService.create(process).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            _$processTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $('.select-process-group-id').select2({
        ajax: {
            delay: 500, // wait 1000 milliseconds before triggering the request
            url: abp.appPath + 'api/services/app/ProcessStepGroup/GetAllListForSelect',
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: data.result
                };
            }
        },
        tags: true
    }).on('select2:select', function (e) {

        var data = e.params.data;
        // Lấy danh sách property tương ứng với sản phẩm
       
    });

     
})(jQuery);