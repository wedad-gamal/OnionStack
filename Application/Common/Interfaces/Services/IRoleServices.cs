using Application.Abstractions;

namespace Application.Common.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IdentityResultDto> CreateRoleAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAsync();
        Task<IdentityResultDto> RemoveRoleAsync(string roleName);
        Task<IdentityResultDto> UpdateRoleAsync(RoleDto role);
        Task<List<UserRoleResultDto>> AddUsersToRoleAsync(string roleName, IEnumerable<UserRoleDto> users);
        Task<RoleDto> GetByNameAsync(string roleName);
        Task<RoleDto> GetByIdAsync(string id);
        Task<IEnumerable<UserRoleDto>> GetAllAsync(string roleName);
        Task<IEnumerable<UserRoleDto>> GetIsAssignedUsersAllAsync(string roleName);


    }

}
