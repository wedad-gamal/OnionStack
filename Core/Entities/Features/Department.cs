namespace Core.Entities.Features
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
