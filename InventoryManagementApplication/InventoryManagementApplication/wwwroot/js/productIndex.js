document.addEventListener('DOMContentLoaded', function () {
    // Sort table function
    function sortTable(columnIndex) {
        const table = document.getElementById("productTable");
        const rows = Array.from(table.querySelectorAll('tbody tr'));

        const isAscending = table.getAttribute("data-sort-direction") === "asc";

        rows.sort((a, b) => {
            const cellA = a.cells[columnIndex].innerText.trim();
            const cellB = b.cells[columnIndex].innerText.trim();

            if (columnIndex === 3) { // If sorting by price (column 3)
                const valueA = parseFloat(cellA.replace(/[^0-9,.-]+/g, "").replace(",", "."));
                const valueB = parseFloat(cellB.replace(/[^0-9,.-]+/g, "").replace(",", "."));

                return isAscending ? valueA - valueB : valueB - valueA;
            } else { // Sorting text columns
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

    // Search functionality
    //document.getElementById('searchInput').addEventListener('keyup', function () {
    //    const searchValue = this.value.toLowerCase();
    //    const table = document.getElementById('productTable');
    //    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

    //    Array.from(rows).forEach(row => {
    //        const cells = row.getElementsByTagName('td');
    //        let rowMatches = false;

    //        // Check if any cell contains the search text
    //        for (let i = 0; i < cells.length; i++) {
    //            const cell = cells[i].innerText.toLowerCase();
    //            if (cell.includes(searchValue)) {
    //                rowMatches = true;
    //                break;
    //            }
    //        }

    //        // Show or hide the row based on whether it matches
    //        row.style.display = rowMatches ? '' : 'none';
    //    });
    //});

    // Optionally, attach the sortTable to the table headers
    document.querySelectorAll('#productTable th a').forEach((header, index) => {
        header.addEventListener('click', function () {
            sortTable(index);
        });
    });
});