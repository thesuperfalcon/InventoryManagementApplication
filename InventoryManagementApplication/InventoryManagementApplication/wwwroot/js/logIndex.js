
const rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

$(document).ready(function () {

    originalRows = Array.from(document.querySelectorAll("#myLogTable tr:not(:first-child)"));
    filteredRows = [...originalRows];

    updateVisibleRows(filteredRows);
    paginateTable();

    // Search functionality
    $("#searchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();

        filteredRows = originalRows.filter(row => {
            return row.innerText.toLowerCase().includes(value);
        });

        updateVisibleRows(filteredRows);
        currentPage = 1;
        paginateTable();
    });

    // Pagination buttons
    $("#prevPage").on("click", function () {
        if (currentPage > 1) {
            currentPage--;
            paginateTable();
        }
    });

    $("#nextPage").on("click", function () {
        if (currentPage < Math.ceil(filteredRows.length / rowsPerPage)) {
            currentPage++;
            paginateTable();
        }
    });
});

// Update the visibleRows array based on the provided rows
function updateVisibleRows(rows) {
    visibleRows = rows; 
    paginateTable(); 
}


// Pagination Function
function paginateTable() {
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;

    // Hide all rows first
    $("#myLogTable tr").hide(); 
    // Show only the rows that fall within the current page
    visibleRows.forEach((row, index) => {
        if (index >= start && index < end) {
            $(row).show();
        }
    });

    // Update the current page number display
    const pageNumberElement = document.getElementById("pageNumber");
    pageNumberElement.innerText = `Page ${currentPage} of ${Math.ceil(filteredRows.length / rowsPerPage)}`;
}


// Sort function for both alphabetical (A-Ö) and numerical (highest to lowest), case-insensitive
function sortTable(columnIndex) {
    const table = document.getElementById("logsTable"); 
    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    // Sort filtered rows based on the specified column
    filteredRows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        // Check if the column is a date (in the format dd/mm/yyyy hh:mm:ss)
        const isDate = columnIndex === 7; // Assuming 'När' is the 8th column (index 7)

        if (isDate) {
            const [dateA, timeA] = cellA.split(' ');
            const [dateB, timeB] = cellB.split(' ');

            const [dayA, monthA, yearA] = dateA.split('/').map(Number);
            const [dayB, monthB, yearB] = dateB.split('/').map(Number);

            const dateObjA = new Date(yearA, monthA - 1, dayA, ...timeA.split(':').map(Number));
            const dateObjB = new Date(yearB, monthB - 1, dayB, ...timeB.split(':').map(Number));

            return isAscending ? dateObjA - dateObjB : dateObjB - dateObjA;
        }

        // Check if the column is numerical
        const isNumber = !isNaN(parseFloat(cellA)) && !isNaN(parseFloat(cellB));

        if (isNumber) {
            return isAscending
                ? parseFloat(cellA) - parseFloat(cellB)
                : parseFloat(cellB) - parseFloat(cellA);
        } else {
            return isAscending
                ? cellA.localeCompare(cellB, 'sv-SE', { sensitivity: 'base' })
                : cellB.localeCompare(cellA, 'sv-SE', { sensitivity: 'base' });
        }
    });

    // Toggle sorting direction for next click
    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

    // Re-append sorted rows to the table
    const tbody = document.getElementById("myLogTable");
    filteredRows.forEach(row => tbody.appendChild(row)); // Reorder based on sorting

    // Reset to page 1 and update pagination
    currentPage = 1;
    paginateTable();
}
