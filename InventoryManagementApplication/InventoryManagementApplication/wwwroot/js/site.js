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

//Accordion
document.addEventListener("DOMContentLoaded", function () {
    var acc = document.getElementsByClassName("accordion");
    var i;
  
    for (i = 0; i < acc.length; i++) {
      acc[i].addEventListener("click", function () {
        this.classList.toggle("active");
  
        var panel = this.nextElementSibling;
        if (panel.style.display === "block") {
          panel.style.display = "none";
          panel.style.maxHeight = null; // Stäng panelen
        } else {
          panel.style.display = "block";
          panel.style.maxHeight = panel.scrollHeight + "px"; // Öppna panelen
        }
      });
    }
  });  