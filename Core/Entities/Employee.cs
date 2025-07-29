namespace Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        // Navigation properties can be added here if needed
        // public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
