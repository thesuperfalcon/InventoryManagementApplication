document.addEventListener('DOMContentLoaded', function () {
    // Search functionality
    setupSearch('searchInput', 'productTable');

    // Initialize any other functionality if needed
});

// Search functionality
function setupSearch(inputId, tableId) {
    document.getElementById(inputId).addEventListener('keyup', function () {
        const searchValue = this.value.toLowerCase().replace(/\s+/g, ''); // Remove spaces from search input
        const table = document.getElementById(tableId);
        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

        Array.from(rows).forEach(row => {
            const cells = row.getElementsByTagName('td');
            let rowMatches = false;

            // Check if any cell contains the search text
            for (let i = 0; i < cells.length; i++) {
                const cell = cells[i].innerText.toLowerCase().replace(/\s+/g, ''); // Remove spaces from cell text
                if (cell.includes(searchValue)) {
                    rowMatches = true;
                    break;
                }
            }

            // Show or hide the row based on whether it matches
            row.style.display = rowMatches ? '' : 'none';
        });
    });
}

function sortTable(tableId, colIndex) {
    const table = document.getElementById(tableId);
    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.rows);
    const isAsc = tbody.getAttribute('data-sort-direction') === 'asc';

    rows.sort((a, b) => {
        const aText = a.cells[colIndex].innerText.trim();
        const bText = b.cells[colIndex].innerText.trim();

        let aNum, bNum;

        // Handle sorting for the "Pris" column (assuming it's the 4th column, index 3)
        if (colIndex === 3) {
            // Remove "kr", spaces, and format properly to compare as numbers
            aNum = parseFloat(aText.replace(' kr', '').replace(/\s+/g, '').replace(',', '.'));
            bNum = parseFloat(bText.replace(' kr', '').replace(/\s+/g, '').replace(',', '.'));

            if (!isNaN(aNum) && !isNaN(bNum)) {
                return isAsc ? aNum - bNum : bNum - aNum;
            }
        }

        // Handle sorting for "Totalt" and "Antal utan lager" columns (index 4 and 5)
        if (colIndex === 4 || colIndex === 5) {
            // Convert to numbers for comparison
            aNum = parseFloat(aText.replace(/\s+/g, ''));  // Remove spaces for thousands separators
            bNum = parseFloat(bText.replace(/\s+/g, ''));

            if (!isNaN(aNum) && !isNaN(bNum)) {
                return isAsc ? aNum - bNum : bNum - aNum;
            }
        }

        // Handle string sorting for other columns
        if (aText === '' && bText === '') return 0;
        if (aText === '') return isAsc ? 1 : -1;
        if (bText === '') return isAsc ? -1 : 1;

        return isAsc ? aText.localeCompare(bText) : bText.localeCompare(aText);
    });

    // Re-append sorted rows
    rows.forEach(row => tbody.appendChild(row));

    // Toggle sort direction
    tbody.setAttribute('data-sort-direction', isAsc ? 'desc' : 'asc');
}
