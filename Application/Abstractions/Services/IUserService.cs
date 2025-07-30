using Core.Interfaces.Identity;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<IdentityResultDto> AssignRoleToUserAsync(string userId, string roleName);
        IEnumerable<IApplicationUser> GetAllUsers();
        Task<IApplicationUser> GetUserByIdAsync(string userId);
        Task<IApplicationUser> GetUserByUserNameAsync(string userName);
        Task<IdentityResultDto> UpdateUserAsync(IApplicationUser user);
        Task DeleteUserAsync(IApplicationUser user);
        Task<IdentityResultDto> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<List<RoleDto>> GetUserRolesDtoAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
