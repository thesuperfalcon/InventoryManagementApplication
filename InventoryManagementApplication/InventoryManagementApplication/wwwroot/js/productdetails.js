function validateForm() {
  const name = document.getElementById("ProductName").value;
  const description = document.getElementById("ProductDescription").value;
  if (!name || !description) {
    alert("Alla fält måste fyllas i!");
    return false;
  }
  return true;
}

const activityLogRowsPerPage = 5;
let activityLogCurrentPage = 1;
let activityLogOriginalRows = [];
let activityLogFilteredRows = [];

$(document).ready(function () {
  activityLogOriginalRows = Array.from(
    document.querySelectorAll("#activityLogTableBody tr")
  );
  activityLogFilteredRows = [...activityLogOriginalRows];

  paginateActivityLogTable();

  $("#activityLogSearchInput").on("keyup", function () {
    var value = $(this).val().toLowerCase();

    activityLogFilteredRows = activityLogOriginalRows.filter((row) => {
      return row.innerText.toLowerCase().includes(value);
    });

    activityLogCurrentPage = 1;
    paginateActivityLogTable();
  });

  $("#activityLogPrevPage").on("click", function () {
    if (activityLogCurrentPage > 1) {
      activityLogCurrentPage--;
      paginateActivityLogTable();
    }
  });

  $("#activityLogNextPage").on("click", function () {
    if (
      activityLogCurrentPage <
      Math.ceil(activityLogFilteredRows.length / activityLogRowsPerPage)
    ) {
      activityLogCurrentPage++;
      paginateActivityLogTable();
    }
  });
});

function paginateActivityLogTable() {
  const start = (activityLogCurrentPage - 1) * activityLogRowsPerPage;
  const end = start + activityLogRowsPerPage;
  document.querySelectorAll("#activityLogTableBody tr").forEach((row) => {
    row.style.display = "none";
  });

  activityLogFilteredRows.slice(start, end).forEach((row) => {
    row.style.display = "";
  });

  const pageNumberElement = document.getElementById("activityLogPageNumber");
  pageNumberElement.innerText = `Sida ${activityLogCurrentPage} av ${Math.ceil(
    activityLogFilteredRows.length / activityLogRowsPerPage
  )}`;
}

function sortActivityLogTable(columnIndex) {
  const table = document.querySelector("#activityLogTable");
  const isAscending = table.getAttribute("data-sort-direction") === "asc";

  activityLogFilteredRows.sort((a, b) => {
    const cellA = a.cells[columnIndex].innerText.trim();
    const cellB = b.cells[columnIndex].innerText.trim();

    const isDate = !isNaN(Date.parse(cellA)) && !isNaN(Date.parse(cellB));
    const isNumber = !isNaN(parseFloat(cellA)) && !isNaN(parseFloat(cellB));

    let comparisonResult;
    if (isDate) {
      comparisonResult = new Date(cellA) - new Date(cellB);
    } else if (isNumber) {
      comparisonResult = parseFloat(cellA) - parseFloat(cellB);
    } else {
      comparisonResult = cellA.localeCompare(cellB, "sv-SE", {
        sensitivity: "base",
      });
    }

    return isAscending ? comparisonResult : -comparisonResult;
  });

  table.setAttribute("data-sort-direction", isAscending ? "desc" : "asc");

  for (let i = 0; i < 4; i++) {
    const icon = document.getElementById(`activityLogSortIcon${i}`);
    if (i === columnIndex) {
      if (isAscending) {
        icon.classList.remove("bi-chevron-expand");
        icon.classList.add("bi-chevron-down");
      } else {
        icon.classList.remove("bi-chevron-down");
        icon.classList.add("bi-chevron-up");
      }
    } else {
      icon.classList.remove("bi-chevron-down", "bi-chevron-up");
      icon.classList.add("bi-chevron-expand");
    }
  }
  const tbody = document.querySelector("#activityLogTableBody");
  activityLogFilteredRows.forEach((row) => tbody.appendChild(row));

  activityLogCurrentPage = 1;
  paginateActivityLogTable();
}
