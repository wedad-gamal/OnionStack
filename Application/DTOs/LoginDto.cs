using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginDto : IModelDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = true;
    }
}
