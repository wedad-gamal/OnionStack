namespace Application.DTOs.Identity;
public class UserWithIdentityDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Identity fields
    public string Email { get; set; }
    public string UserName { get; set; }


    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; } = new List<string>();
}