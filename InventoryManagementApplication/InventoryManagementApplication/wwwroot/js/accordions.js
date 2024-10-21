document.addEventListener("DOMContentLoaded", function () {
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

    var acc3 = document.getElementById("accordion3");
    var panel3 = document.getElementById("panel3");

    if (acc3 && panel3) {
        acc3.addEventListener("click", function () {
            this.classList.toggle("active");
            if (panel3.style.display === "block") {
                panel3.style.display = "none";
            } else {
                panel3.style.display = "block";
            }
        }); 
    }
});
