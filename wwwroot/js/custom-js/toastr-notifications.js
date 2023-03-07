
var toastrNotification = function (message, type) {
    switch (type) {
        case 'success':
            toastr.success(message, type);
            break;
        case 'info':
            toastr.info(message, type);
            break;
        case 'warning':
            toastr.warning(message, type);
            break;
        case 'error':
            toastr.error(message, type);
            break;
        default:
            break;
    }
}