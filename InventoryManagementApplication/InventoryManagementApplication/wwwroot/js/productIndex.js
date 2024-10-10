function formSubmit() {
    var form = document.getElementById('toggleForm');
    console.log('Submitting form...'); 
    form.submit();
}

function sortTable(columnIndex) {
    const table = document.getElementById("productTable");
    const rows = Array.from(table.querySelectorAll('tbody tr')); 

    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    rows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        if (columnIndex === 3) {
            const valueA = parseFloat(cellA.replace(/[^0-9,.-]+/g, "").replace(",", "."));
            const valueB = parseFloat(cellB.replace(/[^0-9,.-]+/g, "").replace(",", "."));

            return isAscending ? valueA - valueB : valueB - valueA;
        } else {
            return isAscending
                ? cellA.localeCompare(cellB, 'sv-SE')
                : cellB.localeCompare(cellA, 'sv-SE');
        }
    });

    const tbody = table.querySelector('tbody');
    tbody.innerHTML = "";
    rows.forEach(row => tbody.appendChild(row));

    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");
}
