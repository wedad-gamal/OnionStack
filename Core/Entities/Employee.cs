using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Employee
    {
        [Key]
        public int UserId { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        // Navigation properties can be added here if needed
        // public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
