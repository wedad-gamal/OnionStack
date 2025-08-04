using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateUserDto : IModelDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAgree { get; set; }
        public List<RoleDto>? Roles { get; set; }
    }
}
