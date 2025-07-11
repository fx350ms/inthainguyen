(function ($) {
    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        console.log(userNotification);
        switch (userNotification.notification.notificationName) {
            case 'Order.Created':
                {
                    //abp.notify.info(userNotification.notification.data.properties.Message,)
                    //abp.notifications.show({
                    //    title: userNotification.notification.data.properties.Title,
                    //    message: userNotification.notification.data.properties.Message,
                    //    type: 'info', // or 'success', 'warning', 'error'
                    //    // Optionally, add a click handler to navigate to a specific page
                    //    click: function (e,a,b,c) {
                    //        // Logic to navigate to a page related to the notification
                    //        debugger;
                    //    }
                    //});

                    abp.notify.info(
                        userNotification.notification.data.properties.Message,
                        userNotification.notification.data.properties.Title,
                        {
                            // Toastr option for handling click on the toast
                            onclick: function () {
                                // Perform your custom action here, e.g.,
                                var href = '/Orders/Detail/' + userNotification.notification.data.properties.OrderId;
                                //    window.location.href = '/Orders/Detail/' + userNotification.notification.data.properties.OrderId;
                                window.open(href, '_blank');
                                // abp.ui.setBusy($('body'));
                            },
                            // Optional: Prevent auto-dismiss if you want the user to click to dismiss
                            sticky: true,
                            // Optional: Allows clicking anywhere on the toast to dismiss
                            // tapToDismiss: true 
                        }
                    );
                    PlaySound("success");
                }
                break;

            default: {
                abp.notifications.showUiNotifyForUserNotification(userNotification)

                break;
            }
        }

        //if (userNotification.notification.data.type === 'Abp.Notifications.LocalizableMessageNotificationData') {
        //}

        //Desktop notification
        Push.create("InTN", {
            body: userNotification.notification.data.properties.Message,
            icon: abp.appPath + 'img/logo.png',
            timeout: 6000,
            onClick: function () {

                var href = '/Orders/Detail/' + userNotification.notification.data.properties.OrderId;
                window.open(href, '_blank');
                window.focus();
                this.close();
            }
        });
    });
    //Abp.Notifications.LocalizableMessageNotificationData
    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function (camelCased = false) {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        if (camelCased && camelCased === true) {
            return convertToCamelCasedObject(obj);
        }

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }

    //Configure validator
    $.validator.setDefaults({
        highlight: (el) => {
            $(el).addClass('is-invalid');
        },
        unhighlight: (el) => {
            $(el).removeClass('is-invalid');
        },
        errorElement: 'p',
        errorClass: 'text-danger',
        errorPlacement: (error, element) => {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    function convertToCamelCasedObject(obj) {
        var newObj, origKey, newKey, value;
        if (obj instanceof Array) {
            return obj.map(value => {
                if (typeof value === 'object') {
                    value = convertToCamelCasedObject(value);
                }
                return value;
            });
        } else {
            newObj = {};
            for (origKey in obj) {
                if (obj.hasOwnProperty(origKey)) {
                    newKey = (
                        origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey
                    ).toString();
                    value = obj[origKey];
                    if (
                        value instanceof Array ||
                        (value !== null && value.constructor === Object)
                    ) {
                        value = convertToCamelCasedObject(value);
                    }
                    newObj[newKey] = value;
                }
            }
        }
        return newObj;
    }

    function initAdvSearch() {
        $('.abp-advanced-search').each((i, obj) => {
            var $advSearch = $(obj);
            setAdvSearchDropdownMenuWidth($advSearch);
            setAdvSearchStopingPropagations($advSearch);
        });
    }

    initAdvSearch();

    $(window).resize(() => {
        clearTimeout(window.resizingFinished);
        window.resizingFinished = setTimeout(() => {
            initAdvSearch();
        }, 500);
    });

    function setAdvSearchDropdownMenuWidth($advSearch) {
        var advSearchWidth = 0;
        $advSearch.each((i, obj) => {
            advSearchWidth += parseInt($(obj).width(), 10);
        });
        $advSearch.find('.dropdown-menu').width(advSearchWidth)
    }

    function setAdvSearchStopingPropagations($advSearch) {
        $advSearch.find('.dd-menu, .btn-search, .txt-search')
            .on('click', (e) => {
                e.stopPropagation();
            });
    }

    $.fn.clearForm = function () {
        var $this = $(this);
        $this.validate().resetForm();
        $('[name]', $this).each((i, obj) => {
            $(obj).removeClass('is-invalid');
        });
        $this[0].reset();
    };
})(jQuery);
