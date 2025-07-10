
(function ($) {
    const _orderService = abp.services.app.order,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#modal-create-order'),
        _$form = _$modal.find('form');

    Dropzone.autoDiscover = false;
    var dropzone;

    var dropFileIds = [];


    InitDropzone();

    function InitDropzone() {

        const dzone = $('.dropzone');

        if (dzone.length === 0) {
            console.error('Không tìm thấy phần tử Dropzone.');
            return;
        }
        if (!dropzone)
            dropzone = new Dropzone(dzone[0], { // Sử dụng dzone[0] để đảm bảo phần tử DOM hợp lệ
                url: abp.appPath + 'api/services/app/FileUpload/UploadFilesAndGetIds', // URL API để xử lý upload file
                //  url: abp.appPath + 'FileUpload/UploadSingleFile', // URL API để xử lý upload file
                paramName: "files",
                method: "post",
                //  uploadMultiple: true,

                //maxFiles: 1, // Giới hạn số lượng file
                maxFilesize: 5, // Giới hạn kích thước file (MB)
                acceptedFiles: ".jpg,.jpeg,.png,.pdf,.doc,.docx", // Các loại file được chấp nhận
                addRemoveLinks: true, // Hiển thị nút xóa file
                dictDefaultMessage: l('DragAndDropFilesHereOrClickToUpload'),
                dictRemoveFile: l('RemoveFile'),
                headers: {
                    "X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val(),// Token CSRF nếu cần
                    "x-xsrf-token": $('input[name="__RequestVerificationToken"]').val()
                },
                success: function (file, response) {
                    file.id = response.result[0];
                    dropFileIds = dropFileIds.concat(response.result);
                    $('#hidden-file-ids').val(dropFileIds.join(','));
                    abp.notify.success(l('FileUploadedSuccessfully'));
                },
                error: function (file, errorMessage) {
                    // Xử lý khi upload thất bại
                    abp.notify.error(l('FailedToUploadFile'));
                },
                removedfile: function (file, a, b, c) {
                    // Xử lý khi xóa file
                    if (file.id) {
                        dropFileIds.pop(file.id);
                    }
                    $('#hidden-file-ids').val(dropFileIds.join(','));
                    abp.notify.info(l('FileRemoved'));
                    file.previewElement.remove();
                },

            });

    }
    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var data = _$form.serializeFormToObject();

        //// Tạo FormData để gửi dữ liệu và tệp
        //var formData = new FormData();
        //var files = $("input[name='Attachments']")[0].files;

        //// Thêm tệp vào FormData
        //for (var i = 0; i < files.length; i++) {
        //    formData.append('Attachments', files[i]);   
        //}

        //// Lấy các dữ liệu từ form và thêm vào FormData

        //for (var key in data) {
        //    formData.append(key, data[key]);
        //}

        //$.ajax({
        //    url: abp.appPath + 'api/services/app/Order/ApproveDesign',
        //    type: 'PUT',
        //    processData: false,
        //    contentType: false,
        //    data: formData,
        //    success: function () {
        //        abp.notify.info(l('SavedSuccessfully'));
        //        delay(1000, () => { window.location.href = '/Orders' });
        //    },
        //    error: function () {
        //        PlaySound('warning'); abp.notify.error(l('SaveFailed'));
        //    },
        //    complete: function () {
        //        abp.ui.clearBusy(_$form);
        //    }
        //});
    });

})(jQuery);
