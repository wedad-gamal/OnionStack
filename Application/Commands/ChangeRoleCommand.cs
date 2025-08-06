namespace Application.Commands
{
    public class ChangeRoleCommand : IRequest<bool>
    {
        public IEnumerable<UserRoleDto> Users { get; set; } = new List<UserRoleDto>();
        public string RoleName { get; set; }
    }

}
