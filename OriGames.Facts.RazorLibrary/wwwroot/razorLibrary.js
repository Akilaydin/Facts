export function showToast(message, title, type) 
{
    if (window.toastr == false){
        return;
    }

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    window.toastr[type](message, title);
}

export function copyToClipboard(value){
    navigator.clipboard.writeText(value);
}

export function setTagsTotal(element, value) {
    var control = document.querySelector(`#${element}`);
    if (control) {
        control.value = value;
    }
}