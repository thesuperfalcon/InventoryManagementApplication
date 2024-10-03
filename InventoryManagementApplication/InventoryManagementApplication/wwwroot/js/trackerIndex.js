function sortTable(tableId, colIndex) {
    const table = document.getElementById(tableId);
    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.rows);
    const isAsc = tbody.getAttribute('data-sort-direction') === 'asc';

    rows.sort((a, b) => {
        const aText = a.cells[colIndex].innerText.trim();
        const bText = b.cells[colIndex].innerText.trim();

        // Check if the column contains numbers
        const aNum = parseFloat(aText.replace(',', '.'));
        const bNum = parseFloat(bText.replace(',', '.'));

        // If both values are numbers, compare numerically
        if (!isNaN(aNum) && !isNaN(bNum)) {
            return isAsc ? aNum - bNum : bNum - aNum;
        }

        // Otherwise, compare as strings
        return isAsc ? aText.localeCompare(bText) : bText.localeCompare(aText);
    });

    // Remove and append sorted rows
    rows.forEach(row => tbody.appendChild(row));

    // Toggle sort direction
    tbody.setAttribute('data-sort-direction', isAsc ? 'desc' : 'asc');
}


function filterTable() {
    const input = document.getElementById('searchInput');
    const filter = input.value.toLowerCase();
    const table = document.getElementById('trackerTable');
    const rows = table.getElementsByTagName('tr');

    for (let i = 1; i < rows.length; i++) {
        const cells = rows[i].getElementsByTagName('td');
        let match = false;

        // Loop through all cells in the row
        for (let j = 0; j < cells.length - 1; j++) { // Exclude last cell (actions)
            const cellValue = cells[j].textContent || cells[j].innerText;
            if (cellValue.toLowerCase().indexOf(filter) > -1) {
                match = true;
                break;
            }
        }

        // Show or hide the row based on the match
        rows[i].style.display = match ? '' : 'none';
    }
}
