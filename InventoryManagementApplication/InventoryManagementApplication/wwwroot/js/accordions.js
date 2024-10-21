document.addEventListener("DOMContentLoaded", function () {
    // Kontrollera om accordion1 och panel1 existerar innan vi försöker lägga till event listeners
    var acc1 = document.getElementById("accordion1");
    var panel1 = document.getElementById("panel1");

    if (acc1 && panel1) {
        acc1.addEventListener("click", function () {
            this.classList.toggle("active");
            if (panel1.style.display === "block") {
                panel1.style.display = "none";
            } else {
                panel1.style.display = "block";
            }
        });
    }

    // Kontrollera om accordion2 och panel2 existerar innan vi försöker lägga till event listeners
    var acc2 = document.getElementById("accordion2");
    var panel2 = document.getElementById("panel2");

    if (acc2 && panel2) {
        acc2.addEventListener("click", function () {
            this.classList.toggle("active");
            if (panel2.style.display === "block") {
                panel2.style.display = "none";
            } else {
                panel2.style.display = "block";
            }
        });
    }


});