// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    asyncGetModal = (url, title) => {
        try {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#user-modal .modal-title').html(title);
                    $('#user-modal .modal-body').html(res.html);
                    $('#user-modal').modal('show')
                },
                error: function (err) {
                    console.warn(err)
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }

    asyncPostModal = (form) => {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $('#viewContent').html(res.html)
                        $('#user-modal').modal('hide');
                    }
                },
                error: function (err) {
                    console.warn(err);
                }
            });
            return false;
        } catch (ex) {
            console.log(ex);
        }
    }
});