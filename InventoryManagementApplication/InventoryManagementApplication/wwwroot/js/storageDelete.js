document.addEventListener('DOMContentLoaded', function () {
    var currentStock = document.getElementById('currentStock').value;
    var errorMessage = document.getElementById('errorMessage').value;
    var deleteMessage = document.getElementById('deleteMessage').value;

    // Get modal and other related elements
    var modalMessageElement = document.getElementById('modalMessage');
    var deleteFooter = document.getElementById('deleteFooter');
    var errorFooter = document.getElementById('errorFooter');
    var deleteButton = document.getElementById('deleteButton');

    // Show the modal when the delete button is clicked
    deleteButton.addEventListener('click', function () {
        var modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));

        // Set the appropriate message and footer based on stock count
        if (currentStock > 0) {
            modalMessageElement.textContent = errorMessage;
            errorFooter.classList.remove('d-none');
            deleteFooter.classList.add('d-none');
        } else {
            modalMessageElement.textContent = deleteMessage;
            deleteFooter.classList.remove('d-none');
            errorFooter.classList.add('d-none');
        }

        // Show the modal
        modal.show();
    });

    // Close modal when clicking 'Ok' in error footer
    document.querySelector('#errorFooter .btn-primary').addEventListener('click', function () {
        var modal = bootstrap.Modal.getInstance(document.getElementById('confirmDeleteModal'));
        modal.hide();
    });
});