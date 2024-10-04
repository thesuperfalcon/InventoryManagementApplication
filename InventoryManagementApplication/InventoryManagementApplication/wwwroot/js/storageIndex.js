function sortTable(tableId, columnIndex) {
    const table = document.getElementById(tableId);
    const rows = Array.from(table.rows).slice(1); // Get all rows except the header

    // Initialize sort direction if not set
    if (!table.hasAttribute("data-sort-direction")) {
        table.setAttribute("data-sort-direction", "asc");
    }

    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    rows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        // Handle sorting for different data types
        if (columnIndex === 1 || columnIndex === 2 || columnIndex === 3) {
            return isAscending
                ? parseFloat(cellA) - parseFloat(cellB)
                : parseFloat(cellB) - parseFloat(cellA);
        } else {
            return isAscending
                ? cellA.localeCompare(cellB, 'sv-SE')
                : cellB.localeCompare(cellA, 'sv-SE');
        }
    });

    // Clear the table body and append sorted rows
    const tbody = table.querySelector('tbody');
    tbody.innerHTML = "";
    rows.forEach(row => tbody.appendChild(row));

    // Toggle sort direction
    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");
}

// Search functionality
document.getElementById('searchInput').addEventListener('keyup', function () {
    const searchValue = this.value.toLowerCase(); // Get the value from the input
    const table = document.getElementById('storageTable');
    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr'); // Get the rows of the table body

    // Loop through the rows and hide those that don't match the search
    Array.from(rows).forEach(row => {
        const cells = row.getElementsByTagName('td');
        let rowMatches = false; // Flag to track if the row matches the search

        // Check each cell in the row
        for (let i = 0; i < cells.length; i++) {
            const cell = cells[i].innerText.toLowerCase(); // Get the text content of the cell
            if (cell.includes(searchValue)) { // Check if it includes the search value
                rowMatches = true; // Set the flag to true if a match is found
                break; // No need to check further cells
            }
        }

        // Show or hide the row based on whether it matches
        row.style.display = rowMatches ? '' : 'none';
    });
});