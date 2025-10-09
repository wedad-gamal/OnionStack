namespace Application.DTOs.Common
{
    public class UserRoleResultDto : IModelDto
    {
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAssigned { get; set; }
        public bool? Succeed { get; set; }
        public string Email { get; set; }
    }
}
