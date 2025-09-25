
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".edit-role-btn").forEach(button => {
        button.addEventListener("click", async () => {
            const roleId = button.getAttribute("data-id");

            try {
                const response = await fetch(`/Role/GetEditModal/${roleId}`);
                const html = await response.text();
                document.getElementById("editRoleModalContainer").innerHTML = html;

                const modalElement = document.getElementById("editRoleModal");
                const form = document.getElementById("editRoleForm");
                const roleNameInput = document.getElementById("editRoleName");
                const roleIdInput = document.getElementById("editRoleId");
                const errorDiv = document.getElementById("editRoleNameError");

                form.addEventListener("submit", async (e) => {
                    e.preventDefault();
                    const roleName = roleNameInput.value.trim();
                    const roleId = roleIdInput.value;
                    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

                    if (!roleName) {
                        roleNameInput.classList.add("is-invalid");
                        errorDiv.style.display = "block";
                        return;
                    }

                    roleNameInput.classList.remove("is-invalid");
                    errorDiv.style.display = "none";

                    try {
                        const response = await fetch(`/api/RoleApi/UpdateRole/${roleId}`, {
                            method: "PUT",
                            headers: {
                                "Content-Type": "application/json",
                                "X-CSRF-TOKEN": token
                            },
                            body: JSON.stringify({ id: roleId, name: roleName })
                        });

                        const result = await response.json();
                        if (result.success) {
                            bootstrap.Modal.getInstance(document.getElementById("editRoleModal")).hide();
                            ToastSuccess("Role updated successfully.");
                            setTimeout(() => {
                                //window.location.href = "/Role"; // or refresh list via AJAX
                                loadRolesTable();
                            }, 1200);
                        } else {
                            ToastError(result.message);
                        }
                    } catch (err) {
                        console.error("Update failed", err);
                        ToastError("Something went wrong.");
                    }
                });
                const modal = new bootstrap.Modal(modalElement);
                modal.show();




            } catch (err) {
                ToastError("Failed to load role editor.");
                console.error(err);
            }
        });
    });
});