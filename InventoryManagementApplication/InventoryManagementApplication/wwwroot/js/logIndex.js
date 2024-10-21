const rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

$(document).ready(function () {
    originalRows = Array.from(document.querySelectorAll("#myLogTable tr"));
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

    // Page selector change event
    $("#pageSelector").on("change", function () {
        currentPage = parseInt($(this).val());
        paginateTable();
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

    // Update the page selector dropdown
    const pageSelector = document.getElementById("pageSelector");
    const totalPages = Math.ceil(filteredRows.length / rowsPerPage);
    pageSelector.innerHTML = ''; // Clear previous options
    for (let i = 1; i <= totalPages; i++) {
        const option = document.createElement("option");
        option.value = i;
        option.text = `Page ${i}`;
        pageSelector.appendChild(option);
    }
    pageSelector.value = currentPage; // Set the current page as selected
}
