let rowsPerPage = 12;
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
    originalRows = Array.from(document.querySelectorAll("#myTable tr"));
    filteredRows = [...originalRows];

    updateVisibleRows(filteredRows);
    paginateTable();

    let pageSelector = document.getElementById('pageSelector');
    pageSelector.addEventListener('change', function () {
        currentPage = parseInt(pageSelector.value);
        paginateTable();
    });


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
        var searchTerms = $(this).val().toLowerCase().split(',').map(term => term.trim());
        var filterRows = [];

        for (var i = 0; i < originalRows.length; i++) {
            var row = originalRows[i];
            var rowText = Array.from(row.cells).map(cell => cell.innerText.toLowerCase()).join(" ");
            $(row).css('display', 'table-row');

            var rowMatches = searchTerms.every(term => {
                if (term.startsWith('från:')) {
                    var storageTerm = term.replace('från:', '').trim();
                    return row.cells[4].innerText.toLowerCase().startsWith(storageTerm); 
                } else if (term.startsWith('till:')) {
                    var storageTerm = term.replace('till:', '').trim();
                    return row.cells[5].innerText.toLowerCase().startsWith(storageTerm); 
                } else if (!isNaN(term) && term.length > 0) {
                    return rowText.split(' ').some(cellText => cellText.startsWith(term));
                } else {
                    return rowText.indexOf(term) >= 0;
                }
            });

            if (!rowMatches) {
                $(row).css('display', 'none');
            }
            else {
                filterRows.push(row);
            }
        }
        updateVisibleRows(filterRows);
        paginateTable();
        updatePageSelector(); // Lägg till detta anrop


        console.log(`Söktermer: ${searchTerms.join(", ")}`);
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

    $("#myTable tr").hide(); // Göm alla rader

    // Visa de filtrerade raderna för den aktuella sidan
    visibleRows.forEach((row, index) => {
        if (index >= start && index < end) {
            $(row).show();
        }
    });

    const pageNumberElement = document.getElementById("pageNumber");
    pageNumberElement.innerText = `Page ${currentPage} of ${Math.ceil(filteredRows.length / rowsPerPage)}`;

    updatePageSelector(); // Anropa denna funktion här om antalet sidor ändras
}


function updatePageSelector() {
    const pageSelector = document.getElementById('pageSelector');
    const totalPages = Math.ceil(filteredRows.length / rowsPerPage);

    // Clear existing options
    pageSelector.innerHTML = '';

    // Add options for each page
    for (let i = 1; i <= totalPages; i++) {
        const option = document.createElement('option');
        option.value = i;
        option.textContent = `Page ${i}`;
        pageSelector.appendChild(option);
    }

    // Set current page in selector
    pageSelector.value = currentPage;
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

function setMovementAttributes(employeeNumber, value, isInitialStorage = false, isDestinationStorage = false) {
    const searchInput = document.getElementById("searchInput");

    resetTableVisibility("leaderboardTable");
    resetTableVisibility("statisticsTable");

    const filters = [];

    if (employeeNumber) filters.push(employeeNumber + ', ');

    if (isInitialStorage) {
        filters.push('från: ' + value + ', '); 
    } else if (isDestinationStorage) {
        filters.push('till: ' + value + ', '); 
    } else if (value) {
        const datePattern = /^\d{4}-\d{2}-\d{2}$/;
        if (datePattern.test(value)) {
            filters.push(value + ", ");
        } else {
            filters.push(value + ', ');
        }
    }

    searchInput.value = filters.join('');

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
    var employeeNumber = document.getElementById('emplyeeNumber').value;

    if (employeeNumber) {
        $('#searchInput').val(`#a:${employeeNumber}.`);

        var event = new Event('input', {
            bubbles: true,
            cancelable: true,
        });
        document.getElementById('searchInput').dispatchEvent(event);
    }
});

