let rowsPerPage = 12;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

function debounce(func, delay) {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), delay);
    };
}

$(document).ready(function () {
    const pageSelector = document.getElementById('pageSelector');

    originalRows = Array.from(document.querySelectorAll("#storageTable tbody tr"));
    filteredRows = [...originalRows];
    updateVisibleRows(filteredRows);
    paginateTable();

    $("#searchInput").on("input", debounce(function () {
        var searchTerms = $(this).val().toLowerCase().split(',').map(term => term.trim());
        filteredRows = originalRows.filter(row => {
            const rowText = Array.from(row.cells).map(cell => cell.innerText.toLowerCase()).join(" ");
            return searchTerms.every(term => rowText.includes(term));
        });
        updateVisibleRows(filteredRows);
        paginateTable();
    }, 300));

    $("#prevPage").on("click", function () {
        if (currentPage > 1) {
            currentPage--;
            updatePageSelector();
            paginateTable();
        }
    });

    $("#nextPage").on("click", function () {
        if (currentPage < Math.ceil(filteredRows.length / rowsPerPage)) {
            currentPage++;
            updatePageSelector();
            paginateTable();
        }
    });

    pageSelector.addEventListener('change', function () {
        currentPage = parseInt(this.value);
        paginateTable();
    });

    function updatePageSelector() {
        pageSelector.value = currentPage;
    }
});

function clearProductSearch() {
    const searchInput = document.getElementById("searchInput");
    searchInput.value = "";
    filteredRows = originalRows;
    updateVisibleRows(filteredRows);
    paginateTable();
}

function updateVisibleRows(rows) {
    visibleRows = rows;
    paginateTable();
}

function paginateTable() {
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;

    $("#storageTable tbody tr").hide();
    visibleRows.forEach((row, index) => {
        if (index >= start && index < end) {
            $(row).show();
        }
    });

    const pageNumberElement = document.getElementById("pageNumber");
    pageNumberElement.innerText = `Sida ${currentPage} av ${Math.ceil(filteredRows.length / rowsPerPage)}`;

    const pageSelector = document.getElementById('pageSelector');
    pageSelector.innerHTML = '';
    for (let i = 1; i <= Math.ceil(filteredRows.length / rowsPerPage); i++) {
        const option = document.createElement('option');
        option.value = i;
        option.text = `Sida ${i}`;
        pageSelector.appendChild(option);
    }
    pageSelector.value = currentPage;
}

function sortTable(tableId, columnIndex) {
    const table = document.getElementById(tableId);
    const rows = Array.from(table.rows).slice(1);

    if (!table.hasAttribute("data-sort-direction")) {
        table.setAttribute("data-sort-direction", "asc");
    }

    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    rows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

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

    const tbody = table.querySelector('tbody');
    tbody.innerHTML = "";
    rows.forEach(row => tbody.appendChild(row));

    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");
}

document.getElementById('searchInput').addEventListener('keyup', debounce(function () {
    const searchValue = this.value.toLowerCase();
    const table = document.getElementById('storageTable');
    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

    Array.from(rows).forEach(row => {
        const cells = row.getElementsByTagName('td');
        let rowMatches = false;

        for (let i = 0; i < cells.length; i++) {
            const cell = cells[i].innerText.toLowerCase();
            if (cell.includes(searchValue)) {
                rowMatches = true;
                break;
            }
        }

        row.style.display = rowMatches ? '' : 'none';
    });
}, 300));
