function FnUploadForm(divName, url) {
    var formData = new FormData(document.querySelector('#upload-form'));
    SubmitForm(formData, url);
}

function FnFailUpload() {
    $('#upload-box').hide();
    $('#upload-box-fail').prop('hidden', false);
}

function FnSuccessUpload() {
    $('#upload-box').hide();
    $('#upload-box-success').prop('hidden', false);
}
function SubmitForm(FormData, url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: FormData,
        dataType: 'json',
        cache: false,
        processData: false,
        contentType: false,
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {
            if (!response.success) {

            } else {

            }
        },
        error: function (err) {
            return response.message;
        }
    });
}

function SubmitImageForm(FormData, url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: FormData,
        dataType: 'json',
        cache: false,
        processData: false,
        contentType: false,
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {
            if (!response.success) {

            } else {

            }
        },
        error: function (err) {
            return response.message;
        }
    });
}