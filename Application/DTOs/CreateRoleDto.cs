using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateRoleDto : IModelDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
