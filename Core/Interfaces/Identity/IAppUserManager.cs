using Core.Results;

namespace Core.Interfaces.Identity
{
    public interface IAppUserManager
    {
        IQueryable<IApplicationUser> Users { get; }
        Task<IdentityResultDto> AddToRoleAsync(IApplicationUser user, string role);
        Task<IApplicationUser> FindByIdAsync(string userId);
        Task<IApplicationUser> FindByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(IApplicationUser user);
        Task<bool> IsInRoleAsync(IApplicationUser user, string role);
        Task<IdentityResultDto> RemoveFromRoleAsync(IApplicationUser user, string role);
    }
}
