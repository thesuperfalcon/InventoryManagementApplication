$(document).ready(function () {
    // Handle click event for generating employee number
    $('#generateEmployeeNumberButton').on('click', function () {
        $.ajax({
            url: '/Identity/Account/Register?handler=GenerateEmployeeNumber', // Call the Razor Page handler
            type: 'GET', // You can also use POST if needed
            success: function (data) {
                if (data && data.employeeNumber) {
                    // Set the generated employee number to the input field
                    $('#employeeNumber').val(data.employeeNumber);
                }
            },
            error: function (xhr, status, error) {
                alert("Error generating employee number. Please try again.");
            }
        });
    });
});