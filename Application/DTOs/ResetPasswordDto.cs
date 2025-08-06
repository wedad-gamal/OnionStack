using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ResetPasswordDto
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
