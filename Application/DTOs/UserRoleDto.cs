namespace Application.DTOs
{
    public class UserRoleDto : IModelDto
    {
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
