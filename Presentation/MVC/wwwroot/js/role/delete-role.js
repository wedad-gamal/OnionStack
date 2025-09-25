
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".confirm-delete-role-btn").forEach(button => {
        button.addEventListener("click", async () => {
            const roleId = button.getAttribute("data-id");

            try {
                const response = await fetch(`/Role/GetConfirmDeleteModal/${roleId}`);
                const html = await response.text();
                document.getElementById("confirmDeleteRoleModalContainer").innerHTML = html;
                const form = document.getElementById("deleteRoleForm");
                const modalElement = document.getElementById("deleteRoleModal");
                

                form.addEventListener("submit", async (e) => {
                    e.preventDefault();
                    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
                    try {
                        const response = await fetch(`/api/RoleApi/${roleId}`, {
                            method: "DELETE",
                            headers: {
                                "Content-Type": "application/json",
                                "X-CSRF-TOKEN": token
                            }                            
                        });

                        const result = await response.json();
                        if (result.success) {
                            const data = result.data;
                            bootstrap.Modal.getInstance(document.getElementById("deleteRoleModal")).hide();
                            ToastSuccess(`Role ${data.Name} deleted successfully.`);
                            setTimeout(() => {
                                //window.location.href = "/Role"; // or refresh list via AJAX
                                loadRolesTable();
                            }, 1200);
                        } else {
                            ToastError(result.message);
                        }
                    } catch (err) {
                        console.error("delete failed", err);
                        ToastError("Something went wrong.", err);
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