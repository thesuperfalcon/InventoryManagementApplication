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
document.addEventListener("DOMContentLoaded", function () {
    // Hantera produktlänkar
    const productLinks = document.querySelectorAll(".product-link");
    productLinks.forEach(link => {
        link.addEventListener("click", function () {
            const productId = this.getAttribute("data-id");
            window.location.href = `/admin/product?id=${productId}`;
        });
    });

    // Hantera lagringslänkar
    const storageLinks = document.querySelectorAll(".storage-link");
    storageLinks.forEach(link => {
        link.addEventListener("click", function () {
            const storageId = this.getAttribute("data-id");
            window.location.href = `/admin/storage?id=${storageId}`;
        });
    });

    // Hantera användarlänkar med specifik URL-struktur
    const userLinks = document.querySelectorAll(".user-link");
    userLinks.forEach(link => {
        link.addEventListener("click", function () {
            const userId = this.getAttribute("data-id");
            window.location.href = `/UserInfo/${userId}`;
        });
    });
});


