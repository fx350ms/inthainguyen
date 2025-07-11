
(function ($) {
    const _orderService = abp.services.app.order,
        _orderAttachmentService = abp.services.app.orderAttachment,
        _fileUploadService = abp.services.app.fileUpload,
        l = abp.localization.getSource('InTN'),
        _$modal = $('#modal-create-order'),
        _$form = _$modal.find('form');

    Dropzone.autoDiscover = false;
    var dropzone;
    var dropFileIds = [];
    InitDropzone();

    function InitDropzone() {

        const dzone = $('.dropzone');
        const orderId = _$form.find('input[name="OrderId"]').val(); // Lấy OrderId từ form
        const attachmentType = _$form.find('input[name="AttachmentType"]').val(); // Lấy FileType từ form
        if (orderId) {
            if (dzone.length === 0) {
                console.error('Không tìm thấy phần tử Dropzone.');
                return;
            }
            if (!dropzone)
                dropzone = new Dropzone(dzone[0], { // Sử dụng dzone[0] để đảm bảo phần tử DOM hợp lệ
                    url: abp.appPath + 'api/services/app/OrderAttachment/UploadAttachments?orderId=' + orderId + '&attachmentType=' + attachmentType, // URL API để xử lý upload file
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
                        debugger;
                        file.id = response.result[0].fileUploadId;
                        abp.notify.success(l('FileUploadedSuccessfully'));
                    },
                    error: function (file, errorMessage) {
                        // Xử lý khi upload thất bại

                        abp.notify.error(l('FailedToUploadFile'));
                    },
                    removedfile: function (file) {
                        // Xử lý khi xóa file
                        if (file.id) {
                            _fileUploadService.deleteWithAttachment(file.id).done(function () {
                                abp.notify.info(l('FileRemoved'));
                                file.previewElement.remove();
                            });
                        }

                    },
                    init: function () {
                        const dz = this;
                        _fileUploadService.geAttachmentstByOrderId(orderId, attachmentType).done(function (response) {

                            if (response && response && response.length > 0) {
                                response.forEach(function (file) {
                                    const mockFile = {
                                        name: file.fileName,
                                        size: file.fileSize,
                                        id: file.id,
                                        type: file.fileType,
                                    };

                                    // Thêm file vào Dropzone
                                    dz.emit("addedfile", mockFile);

                                    // Nếu là hình ảnh, hiển thị thumbnail
                                    if (file.fileType.startsWith("image/")) {
                                        var dataUrl = "data:" + file.fileType + ";base64," + file.fileContent;
                                        dz.emit("thumbnail", mockFile, dataUrl);
                                        var previews = dz.previewsContainer.querySelectorAll('[data-dz-thumbnail]');
                                        previews.forEach(function (img) {
                                            img.style.objectFit = 'contain';
                                            img.style.width = '100%';
                                            img.style.height = '100%';
                                        });
                                    }

                                    // Đánh dấu file là đã upload
                                    dz.emit("complete", mockFile);
                                });
                            }
                        });

                    },
                });
        }
    }


    $('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var data = _$form.serializeFormToObject();
         
    });

})(jQuery);
