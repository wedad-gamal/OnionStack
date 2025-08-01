namespace Application.DTOs
{
    public class RoleDto : IModelDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAssigned { get; set; }
    }
}
