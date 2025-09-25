$.validator.setDefaults({
    errorClass: "",
    validClass: "",

    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
        $(element.form).find("data-valmsg-for='" + element.id + "'").addClass("invalid-feedback").removeClass("valid-feedback");
    },
    unheight: function (element, errorClass, validClass) {
        $(element).addClass("is-valid").removeClass("is-invalid");
        $(element.form).find("data-valmsg-for='" + element.id + "'").addClass("valid-feedback").removeClass("invalid-feedback");
    }
});