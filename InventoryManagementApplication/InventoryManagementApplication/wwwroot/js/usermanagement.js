$(document).ready(function () {
    var adminCount = $('#openRoleChangeModalBtn').data('admin-count');
    var currentRole = $('#openRoleChangeModalBtn').data('current-role');

    $('#openRoleChangeModalBtn').on('click', function (e) {
        e.preventDefault();
        var selectedRole = $('#RoleSelection').val();

        if (!selectedRole || selectedRole.trim() === "") {
            alert("Du måste välja en giltig roll.");
            return false;
        }

        if (currentRole === "" && selectedRole === "Användare") {
              $('#modalRole').text(selectedRole);
            $('#roleChangeModalLabel').text("Användaren har redan denna roll");
            $('#confirmRoleChangeBtn').hide(); 
            $('#roleChangeModal').modal('show');
            return;
        }

        if (selectedRole === currentRole) {
            $('#modalRole').text(selectedRole);
            $('#roleChangeModalLabel').text("Användaren har redan denna roll");
            $('#confirmRoleChangeBtn').hide(); 
            $('#roleChangeModal').modal('show');
            return;
        }
if (currentRole === "Användare" && !selectedRole) {
alert("Användaren har redan ingen tilldelad roll.");
return false;
}

        var isChangingToUser = selectedRole === "Användare";
        if (isChangingToUser && adminCount <= 1) {
            alert("Det måste finnas minst en admin kvar. Du kan inte ta bort adminrollen om det bara finns en admin.");
            return false;
        }

        $('#modalRole').text(selectedRole);
        $('#roleChangeModalLabel').text("Bekräfta Rolländring");
        $('#confirmRoleChangeBtn').show(); 
        $('#roleChangeModal').modal('show');
    });

  $('#confirmRoleChangeBtn').on('click', function () {
var selectedRole = $('#RoleSelection').val();
currentRole = selectedRole; 
$('#roleChangeForm').submit();
});

});

        $(document).ready(function () {
            $('#openDeleteModalBtn').on('click', function () {
                console.log("Delete button clicked!");
                $('#deleteAccountModal').modal('show');
            });
            $('#confirmDeleteBtn').on('click', function () {
                $('#deleteAccountForm').submit();
            });
        });