var inputForm = document.getElementsByTagName('form')[0];

$(document).ready(FnResetForm());

function FnInitializeForm() {
    FnResetForm();
    inputForm.querySelector("#submit-form").addEventListener("click", function (submitButton) {
        $('#' + inputForm.id).submit();
    });

    inputForm.querySelector("input[type='text'").focus();
    inputForm.querySelectorAll("input[type='text'").forEach(function (formElement) {
        formElement.addEventListener('keypress', function (key) {
            if (key.code.toUpperCase() === "ENTER") {
                FnFormOptionInput(formElement);
            }
        });
    });
}

function FnResetForm() {
    inputForm.querySelectorAll("input[type='text'").values = '';
}

function fnToggleForm(callingElement, targetElement) {
    $('#' + callingElement.parentElement.id).prop('hidden', true);
    $('#' + callingElement.parentElement.id).hide();
    $('#' + targetElement).prop('hidden', false);
    $('#' + targetElement).show();
    inputForm = document.getElementById(targetElement);
    FnInitializeForm();
}

function FnFormOptionInput(currentElement) {
    if ($('#' + currentElement.id).val() === '') {
        alert('Please input value for ' + currentElement.id);
        return; //if empty, or has any spaces
    }
    else if ($('#' + currentElement.id).val().includes(" ")) {
        alert('No spaces allowed in ' + currentElement.id);
        return; //if empty, or has any spaces
    }
    else {
        var nextElement = currentElement.nextElementSibling;
        if (nextElement !== null) {
            {
                if (nextElement.type.toUpperCase() == "TEXT") {
                    $('#' + nextElement.id).focus();
                }
            }
        }
    }
}
