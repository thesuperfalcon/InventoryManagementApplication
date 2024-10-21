$(document).ready(function () {
    var isOpen = false;

    $(".toggle-icon").click(function () {
        var target = $(this).data("target");
        var $nestedTable = $(target);

        if ($nestedTable.is(":visible")) {
            $nestedTable.hide();
            $(this).removeClass("bi-dash-square").addClass("bi-plus-square");
        } else {
            $nestedTable.show();
            $(this).removeClass("bi-plus-square").addClass("bi-dash-square");
        }
    });

    $("#toggleAll").click(function () {
        if (isOpen) {
            $(".nested-table-row").hide();
            $(".toggle-icon").removeClass("bi-dash-square").addClass("bi-plus-square");
            $(this).text("Visa alla");
        } else {
            $(".nested-table-row").show();
            $(".toggle-icon").removeClass("bi-plus-square").addClass("bi-dash-square");
            $(this).text("Stäng alla");
        }
        isOpen = !isOpen;
    });

    $("#searchInput").on("input", function () {
        var value = $(this).val().toLowerCase();
        var filters = value.split(',').map(item => item.trim()).filter(item => item);

        if (filters.length === 0) {
            $("tbody tr").show();
            $(".nested-table-row").hide();
            $(".toggle-icon").removeClass("bi-dash-square").addClass("bi-plus-square");
            return;
        }

        $("tbody tr").each(function () {
            var row = $(this);
            var storageMatchFound = false;
            var productMatchFound = false;
            var nestedRow = row.next(".nested-table-row");  
            var productsInStorage = nestedRow.find("tr.product");

            if (row.hasClass("toggle-row")) {
                var storageName = row.find("td:nth-child(1)").text().toLowerCase().trim();
                var productName = nestedRow.find("td:nth-child(1)").text().toLowerCase().trim();  

                var storageFilter = filters.find(f => f.startsWith("lager:"));
                if (storageFilter) {
                    var storageValue = storageFilter.replace("lager:", "").trim();
                    storageMatchFound = storageName.includes(storageValue);
                } else {
                    storageMatchFound = true; 
                }

                var productFilter = filters.find(f => f.startsWith("produkt:"));
                if (productFilter) {
                    var productValue = productFilter.replace("produkt:", "").trim();
                    productsInStorage.each(function () {
                        var productInStorage = $(this).find("td:nth-child(1)").text().toLowerCase().trim();
                        if (productInStorage.includes(productValue)) {
                            productMatchFound = true;
                            $(this).show(); 
                        } else {
    
                            $(this).hide();
                        }
                    });
                } else {
                    productMatchFound = true; 
                }

                if (storageMatchFound && productMatchFound) {
                    row.show(); 
                    nestedRow.show();
                } else {
                    row.hide(); 
                    nestedRow.hide(); 
                }
            }
        });
    });

    $("#clearSearch").click(function () {
        $("#searchInput").val('');
        $("tbody tr").show();
        $(".nested-table-row").hide();
        $(".toggle-icon").removeClass("bi-dash-square").addClass("bi-plus-square");
    });
});
