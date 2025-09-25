namespace Application.DTOs;
public class UpdateUserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }   // allow email change
}
