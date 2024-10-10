$(document).ready(function () {
  $.extend($.validator.messages, {
    required: "Detta fält är obligatoriskt",
  });

  $('[data-bs-toggle="tooltip"]').tooltip({
    trigger: "manual",
    placement: "right",
    delay: { show: 500, hide: 100 },
  });

  $.validator.addMethod(
    "number",
    function (value, element) {
      return this.optional(element) || !isNaN(value);
    },
    "Ange ett giltigt numeriskt värde"
  );

  $("form").on("submit", function (event) {
    var isValid = $(this).valid();
    if (!isValid) {
      event.preventDefault();

      $(this)
        .find("input")
        .each(function () {
          var input = $(this);

          if (!input.valid()) {
            var errorMessage = input.nextAll("span").text();

            input.attr("data-bs-original-title", errorMessage);
            input.tooltip("show");
            input.addClass("is-invalid");

            return false;
          } else {
            input.tooltip("dispose");
            input.removeClass("is-invalid");
          }
        });
      var firstInvalidField = $(this)
        .find("input")
        .filter(function () {
          return !$(this).valid();
        })
        .first();

      firstInvalidField.focus();
    }
  });
  $('input[type="number"]').on("focus", function () {
    if ($(this).hasClass("is-invalid")) {
      $(this).tooltip("show");
    }
  });
  $('input[type="number"]').on("blur", function () {
    $(this).tooltip("hide");
  });
});
