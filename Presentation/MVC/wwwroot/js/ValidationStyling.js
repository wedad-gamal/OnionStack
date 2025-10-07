$.validator.setDefaults({
    errorClass: "is-invalid",
    validClass: "is-valid",

    highlight: function (element, errorClass, validClass) {
        $(element)
            .addClass(errorClass)
            .removeClass(validClass);

        $(element.form)
            .find("[data-valmsg-for='" + element.name + "']")
            .addClass("invalid-feedback")
            .removeClass("valid-feedback");
    },

    unhighlight: function (element, errorClass, validClass) {
        $(element)
            .addClass(validClass)
            .removeClass(errorClass);

        $(element.form)
            .find("[data-valmsg-for='" + element.name + "']")
            .addClass("valid-feedback")
            .removeClass("invalid-feedback");
    }
});
