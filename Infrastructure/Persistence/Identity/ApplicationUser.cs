namespace Infrastructure.Persistence.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public Email EmailAddress { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string? PhoneNumber { get; set; }
        //public Address? Address { get; set; }
        public ApplicationUser() { }

        public ApplicationUser(string userName, Email email)
        {
            UserName = userName;
            EmailAddress = email;
            base.Email = email.Value; // sync with IdentityUser.Email
        }

        // Override setter for Identity’s Email so it syncs with EmailAddress VO
        public override string Email
        {
            get => EmailAddress?.Value;
            set => EmailAddress = new Email(value);
        }
    }
}
