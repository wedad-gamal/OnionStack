namespace Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<IdentityResultDto> CreateRoleAsync(string roleName);
        IEnumerable<RoleDto> Get();
        Task<RoleDto> GetByIdAsync(string id);
        Task<IdentityResultDto> RemoveRoleAsync(string roleName);
        Task<IdentityResultDto> UpdateRoleAsync(RoleDto role);
        Task AddUsersToRoleAsync(string roleName, List<RoleDto> users);

    }

}
