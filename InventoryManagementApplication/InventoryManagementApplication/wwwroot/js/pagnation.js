const rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

$(document).ready(function () {
    originalRows = Array.from(document.querySelectorAll("#myActivityLogTable tr"));
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
    $("#myActivityLogTable tr").hide();

    // Show only the rows that fall within the current page
    visibleRows.forEach((row, index) => {
        if (index >= start && index < end) {
            $(row).show();
        }
    });

    // Update the current page number display
    const pageNumberElement = document.getElementById("pageNumber");
    pageNumberElement.innerText = `Sida ${currentPage} av ${Math.ceil(filteredRows.length / rowsPerPage)}`;
}

// Sort function
function sortTable(columnIndex) {
    const table = document.querySelector("#activityLogTable");
    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    // Sort filtered rows
    filteredRows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        // Check if the column is a date or number
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

    // Toggle sorting direction
    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

    // Re-append sorted rows
    const tbody = document.querySelector("#myActivityLogTable");
    filteredRows.forEach(row => tbody.appendChild(row));

    currentPage = 1;
    paginateTable();
}
