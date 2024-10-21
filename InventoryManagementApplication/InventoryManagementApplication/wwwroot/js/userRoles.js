﻿let rowsPerPage = 12;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

$(document).ready(function () {
    const pageSelector = document.getElementById('pageSelector');

    originalRows = Array.from(document.querySelectorAll("#myTable tr"));
    filteredRows = [...originalRows];
    updateVisibleRows(filteredRows);
    paginateTable();

    $("#userRolesSearchInput").on("input", function () {
        var searchTerms = $(this).val().toLowerCase().split(',').map(term => term.trim());
        filteredRows = originalRows.filter(row => {
            const rowText = Array.from(row.cells).map(cell => cell.innerText.toLowerCase()).join(" ");
            return searchTerms.every(term => rowText.includes(term));
        });
        updateVisibleRows(filteredRows);
        paginateTable();
    });

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

function sortTable(columnIndex) {
    const table = document.getElementById("userRolesTable");
    const tbody = document.getElementById("myTable");
    const rows = Array.from(tbody.rows);
    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    rows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();
        const isNumber = columnIndex === 4 || columnIndex === 5;

        if (isNumber) {
            return isAscending
                ? new Date(cellA) - new Date(cellB)
                : new Date(cellB) - new Date(cellA);
        } else {
            return isAscending
                ? cellA.localeCompare(cellB, 'sv-SE', { sensitivity: 'base' })
                : cellB.localeCompare(cellA, 'sv-SE', { sensitivity: 'base' });
        }
    });

    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");
    rows.forEach(row => tbody.appendChild(row));

    currentPage = 1;
    paginateTable();
}

function clearUserRolesSearch() {
    const userRolesSearchInput = document.getElementById("userRolesSearchInput");
    userRolesSearchInput.value = "";
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

    $("#myTable tr").hide();
    visibleRows.forEach((row, index) => {
        if (index >= start && index < end) {
            $(row).show();
        }
    });

    const pageNumberElement = document.getElementById("pageNumber");
    pageNumberElement.innerText = `Page ${currentPage} of ${Math.ceil(filteredRows.length / rowsPerPage)}`;

    const pageSelector = document.getElementById('pageSelector');
    pageSelector.innerHTML = '';
    for (let i = 1; i <= Math.ceil(filteredRows.length / rowsPerPage); i++) {
        const option = document.createElement('option');
        option.value = i;
        option.text = `Page ${i}`;
        pageSelector.appendChild(option);
    }
    pageSelector.value = currentPage; 
}
