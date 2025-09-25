//document.addEventListener("DOMContentLoaded", () => {
//    loadRolesTable();
//});
function loadRolesTable() {
    
        $.ajax({
            url: '/Role/RolesTable', // Replace with your controller and action
            type: "GET",
            success: function (result) {
                console.log(result);
                $("#rolesTableContainer").html(result); // Update the container with the new HTML
            },
            error: function (xhr, status, error) {
                console.log(xhr)
                console.log(status)
                console.log(error)
                console.error("Error refreshing list:", error);
            }
        });
        
}