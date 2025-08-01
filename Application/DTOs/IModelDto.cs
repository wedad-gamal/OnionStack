using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IModelDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
