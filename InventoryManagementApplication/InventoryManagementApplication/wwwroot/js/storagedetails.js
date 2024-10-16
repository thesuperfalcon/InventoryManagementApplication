(function () {
  // **** Produkttabellen ****
  const productRowsPerPage = 3;
  let productCurrentPage = 1;
  let productOriginalRows = [];
  let productFilteredRows = [];

  // **** Loggtabellen ****
  const logRowsPerPage = 3;
  let logCurrentPage = 1;
  let logOriginalRows = [];
  let logFilteredRows = [];

  $(document).ready(function () {
    // **** Produkttabellen ****
    productOriginalRows = Array.from(
      document.querySelectorAll("#productTableBody tr")
    );
    productFilteredRows = [...productOriginalRows];

    paginateProductTable();

    $("#productSearchInput").on("keyup", function () {
      var value = $(this).val().toLowerCase();

      productFilteredRows = productOriginalRows.filter((row) => {
        return row.innerText.toLowerCase().includes(value);
      });

      productCurrentPage = 1;
      paginateProductTable();
    });

    $("#productPrevPage").on("click", function () {
      if (productCurrentPage > 1) {
        productCurrentPage--;
        paginateProductTable();
      }
    });

    $("#productNextPage").on("click", function () {
      if (
        productCurrentPage <
        Math.ceil(productFilteredRows.length / productRowsPerPage)
      ) {
        productCurrentPage++;
        paginateProductTable();
      }
    });

    // **** Loggtabellen ****
    logOriginalRows = Array.from(document.querySelectorAll("#logTableBody tr"));
    logFilteredRows = [...logOriginalRows];

    paginateLogTable();

    $("#logPrevPage").on("click", function () {
      if (logCurrentPage > 1) {
        logCurrentPage--;
        paginateLogTable();
      }
    });

    $("#logNextPage").on("click", function () {
      if (logCurrentPage < Math.ceil(logFilteredRows.length / logRowsPerPage)) {
        logCurrentPage++;
        paginateLogTable();
      }
    });

    $("#logSearchInput").on("keyup", function () {
      var value = $(this).val().toLowerCase();

      logFilteredRows = logOriginalRows.filter((row) => {
        return row.innerText.toLowerCase().includes(value);
      });

      logCurrentPage = 1;
      paginateLogTable();
    });

    var tooltipTriggerList = [].slice.call(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
      return new bootstrap.Tooltip(tooltipTriggerEl);
    });
  });

  function paginateProductTable() {
    const start = (productCurrentPage - 1) * productRowsPerPage;
    const end = start + productRowsPerPage;

    document.querySelectorAll("#productTableBody tr").forEach((row) => {
      row.style.display = "none";
    });
    productFilteredRows.slice(start, end).forEach((row) => {
      row.style.display = "";
    });

    const pageNumberElement = document.getElementById("productPageNumber");
    pageNumberElement.innerText = `Sida ${productCurrentPage} av ${Math.ceil(
      productFilteredRows.length / productRowsPerPage
    )}`;
  }
  function paginateLogTable() {
    const start = (logCurrentPage - 1) * logRowsPerPage;
    const end = start + logRowsPerPage;

    document.querySelectorAll("#logTableBody tr").forEach((row) => {
      row.style.display = "none";
    });

    logFilteredRows.slice(start, end).forEach((row) => {
      row.style.display = "";
    });

    const pageNumberElement = document.getElementById("logPageNumber");
    pageNumberElement.innerText = `Sida ${logCurrentPage} av ${Math.ceil(
      logFilteredRows.length / logRowsPerPage
    )}`;
  }

  window.sortProductTable = function (columnIndex) {
    const isAscending = $("#productTable").data("sort-direction") === "asc";
    productFilteredRows.sort((a, b) => {
      const cellA = a.cells[columnIndex].innerText.trim();
      const cellB = b.cells[columnIndex].innerText.trim();

      const isNumber = !isNaN(parseFloat(cellA)) && !isNaN(parseFloat(cellB));

      let comparisonResult;
      if (isNumber) {
        comparisonResult = parseFloat(cellA) - parseFloat(cellB);
      } else {
        comparisonResult = cellA.localeCompare(cellB, "sv-SE", {
          sensitivity: "base",
        });
      }

      return isAscending ? comparisonResult : -comparisonResult;
    });

    $("#productTable").data("sort-direction", isAscending ? "desc" : "asc");
    for (let i = 0; i < 3; i++) {
      const icon = document.getElementById(`sortIcon${i}`);
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
    const tbody = document.querySelector("#productTableBody");
    productFilteredRows.forEach((row) => tbody.appendChild(row));

    productCurrentPage = 1;
    paginateProductTable();
  };
  window.sortLogTable = function (columnIndex) {
    const isAscending = $("#logTable").data("sort-direction") === "asc";
    logFilteredRows.sort((a, b) => {
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

    $("#logTable").data("sort-direction", isAscending ? "desc" : "asc");

    for (let i = 0; i < 4; i++) {
      const icon = document.getElementById(`logSortIcon${i}`);
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
    const tbody = document.querySelector("#logTableBody");
    logFilteredRows.forEach((row) => tbody.appendChild(row));

    logCurrentPage = 1;
    paginateLogTable();
  };

  var acc = document.getElementsByClassName("accordion");
  var i;

  for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
      this.classList.toggle("active");

      var panel = this.nextElementSibling;
      if (panel.style.display === "block") {
        panel.style.display = "none";
      } else {
        panel.style.display = "block";
      }
    });
  }
})();
