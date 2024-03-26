
function notifySuccess(message, duration) {
    if (duration) {
        notyf.success({
            message: message,
            duration: duration
        });
    }
    else {
        notyf.success({
            message: message
        });
    }
}
function notifyError(message, duration) {
    if (duration) {
        notyf.error({
            message: message,
            duration: duration
        });
    }
    else {
        notyf.error({
            message: message
        });
    }
}
function notifyWarning(message, duration) {
    if (duration) {
        notyf.open({
            type: 'warning',
            message: message,
            duration: duration
        });
    }
    else {
        notyf.open({
            type: 'warning',
            message: message
        });
    }
}
function notifyInformation(message, duration) {
    if (duration) {
        notyf.open({
            type: 'info',
            message: message,
            duration: duration
        });
    }
    else {
        notyf.open({
            type: 'info',
            message: message
        });
    }
}