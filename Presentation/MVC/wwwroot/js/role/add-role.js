
document.addEventListener("DOMContentLoaded", async () => {
    const form = document.getElementById("addRoleForm");
    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        var roleName = $('#roleName').val().trim();
        if (validateInput(roleName)) {
           await AddRole(roleName);
        }
    });
});
function validateInput(roleName) {
    if (!roleName) {
        $("#roleName").addClass("is-invalid");
        return false;
    }

    // Check if role already exists in table (case-insensitive)
    let exists = false;
    $("#roleTable tr").each(function () {
        let nameCell = $(this).find("td:nth-child(2)").text().trim();
        if (nameCell.toLowerCase() === roleName.toLowerCase()) {
            exists = true;
            return false; // break loop
        }
    });

    if (exists) {
        $("#roleName").addClass("is-invalid");
        $("#roleNameError").text("This role already exists").show();
        return false;
    }

    $("#roleName").removeClass("is-invalid");
    $("#roleNameError").text("Role name is required").hide();
    return true;
}

async function AddRole(roleName) {    

    var token = $('input[name="__RequestVerificationToken"]').val();

    try {
        const response = await fetch(`/api/RoleApi`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-CSRF-TOKEN": token
            },
            body: JSON.stringify({ name: roleName })
        });

        const result = await response.json();
        console.log(result);
        if (result.success) {   
            var data = result.data;
            $("#roleName").val(""); // clear input
            ToastSuccess(`Role "${data.name}" has been added successfully.`);
            setTimeout(() => {
                // window.location.href = "/Role"; // or refresh list via AJAX
                loadRolesTable();
            }, 1200);

        } else {
            ToastError('Invalid Data');
        }
    } catch (err) {
        console.error("Add Role failed", err);
        ToastError("Something went wrong.");
    }    
}