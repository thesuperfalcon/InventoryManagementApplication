
    // Sort table function
    function sortTable(columnIndex) {
        const table = document.getElementById("productTable");
        const rows = Array.from(table.querySelectorAll('tbody tr'));

        // Get current sorting direction (ascending or descending)
        const isAscending = table.getAttribute("data-sort-direction") === "asc";

        rows.sort((a, b) => {
            // Get the text content of the corresponding cells for both rows
            const cellA = a.cells[columnIndex].textContent.trim();
            const cellB = b.cells[columnIndex].textContent.trim();

            if (columnIndex === 3) { // Price column (index 3)
                // Remove non-numeric characters and parse as float
                const valueA = parseFloat(cellA.replace(/[^0-9,.-]+/g, "").replace(",", "."));
                const valueB = parseFloat(cellB.replace(/[^0-9,.-]+/g, "").replace(",", "."));

                // Ensure the parsed values are valid numbers, otherwise default to 0
                return isAscending ? (valueA - valueB) : (valueB - valueA);
            } else if (columnIndex === 4 || columnIndex === 5) { // Numeric columns (Total and Stock)
                const valueA = parseInt(cellA, 10) || 0;
                const valueB = parseInt(cellB, 10) || 0;
                return isAscending ? (valueA - valueB) : (valueB - valueA);
            } else if (columnIndex === 6 || columnIndex === 7) { // Date columns (Created, Updated)
                const dateA = new Date(cellA);
                const dateB = new Date(cellB);
                return isAscending ? (dateA - dateB) : (dateB - dateA);
            } else {
                // For text columns, use localeCompare for proper string sorting
                return isAscending
                    ? cellA.localeCompare(cellB, 'sv-SE')
                    : cellB.localeCompare(cellA, 'sv-SE');
            }
        });

        // Rebuild table with sorted rows
        const tbody = table.querySelector('tbody');
        tbody.innerHTML = "";
        rows.forEach(row => tbody.appendChild(row));

        // Toggle sort direction
        table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");
}


document.addEventListener('DOMContentLoaded', function () {
     //Search functionality
    document.getElementById('searchInput').addEventListener('keyup', function () {
        const searchValue = this.value.toLowerCase();
        const table = document.getElementById('productTable');
        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

        Array.from(rows).forEach(row => {
            const cells = row.getElementsByTagName('td');
            let rowMatches = false;

            // Check if any cell contains the search text
            for (let i = 0; i < cells.length; i++) {
                const cell = cells[i].innerText.toLowerCase();
                if (cell.includes(searchValue)) {
                    rowMatches = true;
                    break;
                }
            }

            // Show or hide the row based on whether it matches
            row.style.display = rowMatches ? '' : 'none';
        });
    });
});