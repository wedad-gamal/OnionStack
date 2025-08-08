namespace Core.Interfaces
{
    public interface IApplicationUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int? Age { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Domain methods
        public string FullName => $"{FirstName} {LastName}";
    }
}
