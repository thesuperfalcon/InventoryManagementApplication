// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Genererar ett unikt anställningsnummer till användaren
document.addEventListener("DOMContentLoaded", function () {
    var container = document.getElementById("registerContainer") || document.getElementById("userInfoContainer");
    if (container) {
        var checkEmployeeNumberUrl = container.getAttribute("data-check-url");

        $("#generateEmployeeNumber").on("click", function () {
            generateUniqueEmployeeNumber();
        });

        function generateUniqueEmployeeNumber() {
            var employeeNumber = Math.floor(Math.random() * (9999 - 1000 + 1)) + 1000;

            $.ajax({
                url: checkEmployeeNumberUrl,
                type: "GET",
                data: { employeeNumber: employeeNumber },
                success: function (data) {
                    if (data) {
                        $("#employeeNumber").val(employeeNumber);
                    } else {
                        generateUniqueEmployeeNumber();
                    }
                },
            });
        }
    }
});


//document.addEventListener("DOMContentLoaded", function () {
//    var container = document.getElementById("productContainer");
//    if (container) {
//        var checkArticleNumberUrl = container.getAttribute("data-check-url");

//        $("#generateArticleNumber").on("click", function () {
//            generateUniqueArticleNumber();
//        });

//        function generateUniqueArticleNumber() {
//            var articleNumber = Math.floor(Math.random() * (9999 - 1000 + 1)) + 1000;

//            $.ajax({
//                url: checkArticleNumberUrl,
//                type: "GET",
//                data: { articleNumber: articleNumber },
//                success: function (data) {
//                    if (data) {
//                        $("#articleNumber").val(articleNumber);
//                    } else {
//                        generateUniqueArticleNumber();
//                    }
//                },
//            });
//        }
//    }
//});
