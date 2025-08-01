namespace Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<IdentityResultDto> CreateRoleAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAsync();
        Task<IdentityResultDto> RemoveRoleAsync(string roleName);
        Task<IdentityResultDto> UpdateRoleAsync(RoleDto role);
        Task AddUsersToRoleAsync(string roleName, List<RoleDto> users);
        Task<RoleDto> GetByNameAsync(string roleName);
        Task<RoleDto> GetByIdAsync(string id);

    }

}
