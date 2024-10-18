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
// Sort function for both alphabetical (A-Ö) and numerical (highest to lowest), case-insensitive
// Sort function for alphabetical, numerical, and date (Tidpunkt) sorting
function sortTable(columnIndex) {
    const table = document.getElementById("logsTable");
    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    filteredRows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        console.log("cellA:", cellA);
        console.log("cellB:", cellB);

        // Kontrollera om vi sorterar efter 'Tidpunkt' kolumn (index 5)
        if (columnIndex === 5) {
            const dateA = parseCustomDate(cellA);
            const dateB = parseCustomDate(cellB);

            console.log("dateA:", dateA);
            console.log("dateB:", dateB);

            return isAscending ? dateA - dateB : dateB - dateA;
        }

        // Kontrollera om kolumnen är numerisk
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

    // Rensa aktuella rader och lägg till sorterade rader
    const tbody = document.getElementById("myLogTable");
    tbody.innerHTML = "";
    filteredRows.forEach(row => tbody.appendChild(row));

    // Växla sorteringsriktning
    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

    // Återställ till sida 1 och uppdatera paginering
    currentPage = 1;
    paginateTable();
}

function parseCustomDate(dateString) {
    const [datePart, timePart] = dateString.split(' ');
    const [day, month, year] = datePart.split('-').map(Number); 
    const [hours, minutes, seconds] = timePart.split(':').map(Number);

    return new Date(year, month - 1, day, hours, minutes, seconds);
}
