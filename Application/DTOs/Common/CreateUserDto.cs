using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Common
{
    public class CreateUserDto : IModelDto
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? ProfilePictureUrl { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
