// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    // Hantera produktlänkar
    const productLinks = document.querySelectorAll(".product-link");
    productLinks.forEach(link => {
        link.addEventListener("click", function () {
            const productId = this.getAttribute("data-id");
            window.location.href = `/admin/product/Details?id=${productId}`;
        });
    });

    // Hantera lagringslänkar
    const storageLinks = document.querySelectorAll(".storage-link");
    storageLinks.forEach(link => {
        link.addEventListener("click", function () {
            const storageId = this.getAttribute("data-id");
            window.location.href = `/admin/storage/Details?id=${storageId}`;
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


