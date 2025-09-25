namespace MVC.ViewModels;

public class UserRoleViewModel
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public bool IsAssigned { get; set; } = false;
}
