function ToastSuccess(message) {
    $.toast({
        heading: 'Success',
        text: message,
        showHideTransition: 'slide',
        icon: 'success',
        position: 'bottom-right'
    });
}

function ToastError(message) {
    $.toast({
        heading: 'Error',
        text: message,
        showHideTransition: 'fade',
        icon: 'error',
        position: 'bottom-right'
    });
}

