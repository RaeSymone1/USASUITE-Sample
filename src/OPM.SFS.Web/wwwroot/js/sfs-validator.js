jQuery.validator.setDefaults({
    highlight: function (element, errorClass) {
        if (element.type === 'text' || element.type === "password" || element.type === "textarea" || element.type === "select-one") {
            $(element).removeClass('usa-input-success');
            $(element).closest('.validate-input').addClass('usa-input-error');
            $(element).closest('.required-input').trigger('cssClassChanged');
        }
    },
    unhighlight: function (element, errorClass, validClass) {
        if (element.type === 'text' || element.type === "password" || element.type === "textarea" || element.type === "select-one") {
            var success = false;
            if ($(element).parent().hasClass('usa-input-error')) {
                success = true;
            }
            $(element).closest('.validate-input').removeClass('usa-input-error');
            $(element).closest('.validate-input').trigger('cssClassChanged');
            $(element).siblings(".field-validation-error").hide();
            if (success === true && !$(element).hasClass('usaj-no-success-style')) {
                $(element).addClass('usa-input-success');
            }
        }
    }
});

//handle server-side validations
$(function () {
    $('.field-validation-error').each(function () {
        if (!$(this).parent().hasClass('usa-input-error') && $(this).parent().hasClass('required-input')) {
            $(this).parent().addClass('usa-input-error');
        }
    });
});