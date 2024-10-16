let rowsPerPage = 10;
let currentPage = 1;
let originalRows = [];
let filteredRows = [];
let visibleRows = [];

function toggleHelp() {
    var helpPopup = document.getElementById("helpPopup");
    if (helpPopup.style.display === "none" || helpPopup.style.display === "") {
        helpPopup.style.display = "block";
    } else {
        helpPopup.style.display = "none";
    }
}

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


    $("#leaderboardSearchInput").on("input", function () {
        var value = $(this).val().toLowerCase();
        console.log(`Inmatat värde: "${value}"`);

        $("#leaderboardTable tbody tr.search").hide();
        $("#leaderboardTable tbody tr.nested-table-row").hide();

        let hasVisibleRows = false;

        $("#leaderboardTable tbody tr.search").each(function () {
            var employeeNumber = $(this).find("td:eq(0)").text().toLowerCase();
            var matchFound = employeeNumber.indexOf(value) > -1;

            if (matchFound) {
                $(this).show();
                hasVisibleRows = true;

                $(this).next(".nested-table-row").show();
            }
        });


        if (hasVisibleRows) {
            $("#leaderboardTable thead").show();
        } else {
            $("#leaderboardTable thead").hide();
        }
    });

    $("#searchInput").on("input", function () {
        var value = $(this).val().toLowerCase();

        var cleanedValue = value.replace(/[.,]+$/, '');
        console.log(`Inmatat värde (rensat): ${cleanedValue}`);

        var filters = cleanedValue.split(',').map(item => item.trim()).filter(item => item);
        console.log(`Filter: ${JSON.stringify(filters)}`);

        if (filters.length === 0) {
            filteredRows = originalRows;
            console.log("Inga filter, visar alla rader");
        } else {
            filteredRows = originalRows.filter(row => {
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
                        case "#a":
                            return exactMatch
                                ? row.cells[1].innerText.toLowerCase() === searchTerm
                                : row.cells[1].innerText.toLowerCase().includes(searchTerm);
                        case "#p":
                            return exactMatch
                                ? row.cells[2].innerText.toLowerCase() === searchTerm
                                : row.cells[2].innerText.toLowerCase().includes(searchTerm);
                        case "#pf":
                            return exactMatch
                                ? row.cells[3].innerText === searchTerm
                                : row.cells[3].innerText.includes(searchTerm);
                        case "#ff":
                            return exactMatch
                                ? row.cells[4].innerText.toLowerCase() === searchTerm
                                : row.cells[4].innerText.toLowerCase().includes(searchTerm);
                        case "#ft":
                            return exactMatch
                                ? row.cells[5].innerText.toLowerCase() === searchTerm
                                : row.cells[5].innerText.toLowerCase().includes(searchTerm);
                        case "#n":
                            return exactMatch
                                ? row.cells[6].innerText === searchTerm
                                : row.cells[6].innerText.includes(searchTerm);
                        default:
                            return true;
                    }
                });
            });
        }

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
        closeAllExpandedRows();
    } else {
        content1.style.display = "none";
        content2.style.display = "block";
        button.innerText = "Visa Leaderboard";
    }
}

function closeAllExpandedRows() {
    const nestedRows = document.querySelectorAll('.nested-table-row');
    nestedRows.forEach(row => {
        row.style.display = 'none';
    });

    const toggleIcons = document.querySelectorAll('.toggle-icon i');
    toggleIcons.forEach(icon => {
        icon.classList.remove('bi-dash-square');
        icon.classList.add('bi-plus-square');
    });
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

function setEmployeeNumber(employeeNumber) {
    const searchInput = document.getElementById("searchInput");
    searchInput.value = `#a:${employeeNumber}., `;

    const event = new Event('input', {
        bubbles: true,
        cancelable: true,
    });
    searchInput.dispatchEvent(event);

    toggleContent();
}

function clearLeaderboardSearch() {
    const leaderboardSearchInput = document.getElementById("leaderboardSearchInput");
    leaderboardSearchInput.value = "";

    const event = new Event('input', {
        bubbles: true,
        cancelable: true,
    });
    leaderboardSearchInput.dispatchEvent(event);
}

function setMovementAttributes(employeeNumber, productName, quantity, initialStorageName, destinationStorageName, moved) {
    const searchInput = document.getElementById("searchInput");

    const leaderboardSearchInput = document.getElementById("leaderboardSearchInput");
    const statisticsSearchInput = document.getElementById("searchInput");

    resetTableVisibility("leaderboardTable");
    resetTableVisibility("statisticsTable");

    const filters = [];
    if (employeeNumber) filters.push(`#a: ${employeeNumber}.,`);
    if (productName) filters.push(`#p: ${productName}.,`);
    if (quantity) filters.push(`#pf: ${quantity}.,`);
    if (initialStorageName) filters.push(`#ff: ${initialStorageName}.,`);
    if (destinationStorageName) filters.push(`#ft: ${destinationStorageName}.,`);
    if (moved) filters.push(`#n: ${moved},`);

    searchInput.value = filters.join(' ');

    const event = new Event('input', {
        bubbles: true,
        cancelable: true,
    });
    const isStatisticsVisible = document.getElementById("content1").style.display !== "none";

    if (isStatisticsVisible) {
        toggleContent();
    }
    searchInput.dispatchEvent(event);

    console.log("Söksträng:", searchInput.value);

}

function resetTableVisibility(tableId) {
    const table = document.getElementById(tableId);
    const rows = table.getElementsByTagName("tr");

    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = "";
    }
}


$(document).ready(function () {
    // Retrieve the employee number from the model
    var employeeNumber = document.getElementById('emplyeeNumber').value;

    // Check if the employee number is available
    if (employeeNumber) {
        // Set the search input with the appropriate format
        $('#searchInput').val(`#a:${employeeNumber}.`);

        // Create and dispatch an input event to trigger filtering
        var event = new Event('input', {
            bubbles: true,
            cancelable: true,
        });
        document.getElementById('searchInput').dispatchEvent(event);
    }
});

