namespace Web.ViewModels;

public class AssignRoleToUsersViewModel
{
    public List<UserRoleViewModel> Users { get; set; } = new List<UserRoleViewModel>();
    public string RoleName { get; set; }
}
