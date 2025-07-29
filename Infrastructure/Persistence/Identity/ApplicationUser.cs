using Core.Identity;


namespace Infrastructure.Persistence.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
