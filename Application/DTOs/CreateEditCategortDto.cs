using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;
public class CreateEditCategortDto
{
    public int? Id { get; set; } = default;
    [Required(ErrorMessage = "Name is Required")]
    public string Name { get; set; }
}
