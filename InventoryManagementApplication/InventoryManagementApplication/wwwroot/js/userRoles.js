let rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

$(document).ready(function () {
    let select = document.getElementById('pageAmount');
    select.addEventListener('change', function () {
        rowsPerPage = parseInt(select.value);
        currentPage = 1;
        paginateTable();
    });

    originalRows = Array.from(document.querySelectorAll("#myTable tr"));
    filteredRows = [...originalRows];
    updateVisibleRows(filteredRows);
    paginateTable();

    $("#userRolesSearchInput").on("input", function () {
        var value = $(this).val().toLowerCase().replace(/[.,]+$/, '');
        var filters = value.split(',').map(item => item.trim()).filter(item => item);

        filteredRows = filters.length === 0
            ? originalRows
            : originalRows.filter(row => {
                return filters.every(filter => {
                    let [key, searchTerm] = filter.split(':').map(item => item.trim());
                    searchTerm = searchTerm.toLowerCase();
                    const exactMatch = searchTerm.endsWith('.');
                    if (exactMatch) searchTerm = searchTerm.slice(0, -1).trim();

                    switch (key) {
                        case "#a": return exactMatch ? row.cells[0].innerText.toLowerCase() === searchTerm : row.cells[0].innerText.toLowerCase().includes(searchTerm);
                        case "#f": return exactMatch ? row.cells[1].innerText.toLowerCase() === searchTerm : row.cells[1].innerText.toLowerCase().includes(searchTerm);
                        case "#e": return exactMatch ? row.cells[2].innerText.toLowerCase() === searchTerm : row.cells[2].innerText.toLowerCase().includes(searchTerm);
                        case "#r": return exactMatch ? row.cells[3].innerText === searchTerm : row.cells[3].innerText.includes(searchTerm);
                        case "#s": return exactMatch ? row.cells[4].innerText.toLowerCase() === searchTerm : row.cells[4].innerText.toLowerCase().includes(searchTerm);
                        case "#u": return exactMatch ? row.cells[5].innerText.toLowerCase() === searchTerm : row.cells[5].innerText.toLowerCase().includes(searchTerm);
                        default: return true;
                    }
                });
            });

        updateVisibleRows(filteredRows);
    });

    function updateVisibleRows(rows) {
        visibleRows = rows;
        paginateTable();
    }

    function paginateTable() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        $("#myTable tr").hide();
        visibleRows.forEach((row, index) => {
            if (index >= start && index < end) {
                $(row).show();
            }
        });

        const pageNumberElement = document.getElementById("pageNumber");
        pageNumberElement.innerText = `Page ${currentPage} of ${Math.ceil(filteredRows.length / rowsPerPage)}`;
    }

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

function clearUserRolesSearch() {
    const userRolesSearchInput = document.getElementById("userRolesSearchInput");
    userRolesSearchInput.value = "";

    const event = new Event('input', {
        bubbles: true,
        cancelable: true,
    });
    userRolesSearchInput.dispatchEvent(event);
}