using Application.DTOs.Common;

namespace Application.Abstraction.Interfaces.Common
{
    public interface IRoleService
    {
        Task<IdentityResultDto> CreateRoleAsync(RoleDto roleDto);
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
