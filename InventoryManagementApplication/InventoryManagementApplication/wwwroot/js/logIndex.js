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
        const value = $(this).val().toLowerCase();

        filteredRows = originalRows.filter(row => {
            return row.innerText.toLowerCase().includes(value);
        });

        currentPage = 1; // Reset to first page
        updateVisibleRows(filteredRows);
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

// Update the visible rows based on the provided rows
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

    // Sort based on the specified column
    filteredRows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        // Check if the column is a date
        const isDate = columnIndex === 5; // Assuming 'Tidpunkt' is the 6th column (index 5)

        console.log('cellA:', cellA);
        console.log('cellB:', cellB);

        if (isDate) {
            const dateA = parseCustomDate(cellA);
            const dateB = parseCustomDate(cellB);
            return isAscending ? dateA - dateB : dateB - dateA;
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

    // Clear the current rows and re-append sorted rows
    const tbody = document.getElementById("myLogTable");
    tbody.innerHTML = ""; // Clear current rows
    filteredRows.forEach(row => tbody.appendChild(row)); // Append sorted rows

    // Toggle sort direction
    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

    // Reset to page 1 and update pagination
    currentPage = 1;
    paginateTable();
}

// Function to parse the custom date format 'dd/mm/yyyy hh:mm:ss'
function parseCustomDate(dateString) {
    const [datePart, timePart] = dateString.split(' ');
    const [day, month, year] = datePart.split('/').map(Number);
    const [hours, minutes, seconds] = timePart.split(':').map(Number);

    // Create a new Date object
    return new Date(year, month - 1, day, hours, minutes, seconds);
}