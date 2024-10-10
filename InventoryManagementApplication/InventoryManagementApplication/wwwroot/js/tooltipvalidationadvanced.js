$(document).ready(function () {
  $.extend($.validator.messages, {
    required: "Detta fält är obligatoriskt",
  });

  $('[data-bs-toggle="tooltip"]').tooltip({
    trigger: "manual",
    placement: "right",
    delay: { show: 500, hide: 100 },
  });

  $("form").each(function () {
    $(this).validate({
      submitHandler: function (form) {
        form.submit();
      },

      showErrors: function (errorMap, errorList) {
        this.currentElements.tooltip("hide").removeClass("is-invalid");

        $.each(errorList, function (index, error) {
          var element = $(error.element);

          var customErrorMessage =
            element.nextAll("span").text() || error.message;

          element
            .attr("data-bs-original-title", customErrorMessage)
            .addClass("is-invalid")
            .tooltip("show");
        });
      },

      success: function (label, element) {
        $(element).tooltip("hide").removeClass("is-invalid");
      },

      rules: {
        // Valideringsregler (lägg till vid behov)
      },
    });
  });

  $(document).on("focus", '[data-bs-toggle="tooltip"]', function () {
    if ($(this).hasClass("is-invalid")) {
      $(this).tooltip("show");
    }
  });

  $(document).on("blur", '[data-bs-toggle="tooltip"]', function () {
    $(this).tooltip("hide");
  });
});