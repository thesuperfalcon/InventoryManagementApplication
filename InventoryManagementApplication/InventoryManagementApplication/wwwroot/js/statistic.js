const rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

  function toggleHelp() {
        var popup = document.getElementById("helpPopup");
        popup.style.display = popup.style.display === "none" || popup.style.display === "" ? "flex" : "none";
}

$(document).ready(function () {
    originalRows = Array.from(document.querySelectorAll("#myTable tr:not(:first-child)"));
    filteredRows = [...originalRows];

    updateVisibleRows(filteredRows);
    paginateTable();

    $("#searchInput").on("input", function () {
        var value = $(this).val().toLowerCase();
        console.log(`Inmatat värde: ${value}`);

        var filters = value.split(',').map(item => item.trim()).filter(item => item);
        console.log(`Filter: ${JSON.stringify(filters)}`);

        filteredRows = originalRows.filter(row => {
            console.log(`Rada: ${JSON.stringify(row.cells)}`);

            if (filters.length === 1 && !filters[0].includes(':')) {
                return (
                    row.cells[1].innerText.toLowerCase().includes(filters[0]) ||
                    row.cells[2].innerText.toLowerCase().includes(filters[0]) ||
                    row.cells[3].innerText.includes(filters[0]) ||
                    row.cells[4].innerText.toLowerCase().includes(filters[0]) ||
                    row.cells[5].innerText.toLowerCase().includes(filters[0]) ||
                    row.cells[6].innerText.includes(filters[0])
                );
            }

            return filters.every(filter => {
                let [key, searchTerm] = filter.split(':').map(item => item.trim());
                searchTerm = searchTerm.toLowerCase();

                const exactMatch = searchTerm.endsWith('.');
                if (exactMatch) {
                    searchTerm = searchTerm.slice(0, -1).trim();
                }

                console.log(`Nyckel: ${key}, Sökterm: ${searchTerm}, Exakt matchning: ${exactMatch}`);

                switch (key) {
                    case "#e":
                        return exactMatch
                            ? row.cells[1].innerText.toLowerCase() === searchTerm
                            : row.cells[1].innerText.toLowerCase().includes(searchTerm);
                    case "#p":
                        return exactMatch
                            ? row.cells[2].innerText.toLowerCase() === searchTerm
                            : row.cells[2].innerText.toLowerCase().includes(searchTerm);
                    case "#k":
                        return exactMatch
                            ? row.cells[3].innerText === searchTerm
                            : row.cells[3].innerText.includes(searchTerm);
                    case "#il":
                        return exactMatch
                            ? row.cells[4].innerText.toLowerCase() === searchTerm
                            : row.cells[4].innerText.toLowerCase().includes(searchTerm);
                    case "#dl":
                        return exactMatch
                            ? row.cells[5].innerText.toLowerCase() === searchTerm
                            : row.cells[5].innerText.toLowerCase().includes(searchTerm);
                    case "#d":
                        return exactMatch
                            ? row.cells[6].innerText === searchTerm
                            : row.cells[6].innerText.includes(searchTerm);
                    default:
                        return true;
                }
            });
        });

        console.log(`Filtrerade rader: ${filteredRows.length}`);
        updateVisibleRows(filteredRows);
        currentPage = 1;
        paginateTable();
    });







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

    $(".toggle-icon").on("click", function () {
        const target = $(this).data("target");
        const icon = $(this).find("i");

        $(target).toggle();

        if ($(target).is(":visible")) {
            icon.removeClass("bi-plus-square").addClass("bi-dash-square");
        } else {
            icon.removeClass("bi-dash-square").addClass("bi-plus-square");
        }
    });
});

function toggleContent() {
    const content1 = document.getElementById("content1");
    const content2 = document.getElementById("content2");
    const button = document.getElementById("toggleButton");

    if (content1.style.display === "none") {
        content1.style.display = "block";
        content2.style.display = "none";
        button.innerText = "Visa Statistik";
    } else {
        content1.style.display = "none";
        content2.style.display = "block";
        button.innerText = "Visa Leaderboard";
    }
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
}

function sortTable(columnIndex) {
    const table = document.getElementById("statisticsTable");
    const isAscending = table.getAttribute("data-sort-direction") === "asc";

    filteredRows.sort((a, b) => {
        const cellA = a.cells[columnIndex].innerText.trim();
        const cellB = b.cells[columnIndex].innerText.trim();

        const isNumber = columnIndex === 3;

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

    table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

    const tbody = document.getElementById("myTable");
    filteredRows.forEach(row => tbody.appendChild(row));

    currentPage = 1;
    paginateTable();
}