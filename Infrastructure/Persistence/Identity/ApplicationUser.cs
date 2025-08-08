namespace Infrastructure.Persistence.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? Age { get; set; } = null;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        // Conversion method to Domain entity
        public User ToDomainUser() => new()
        {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            UserName = UserName,
            ProfilePictureUrl = ProfilePictureUrl
        };
    }
}
