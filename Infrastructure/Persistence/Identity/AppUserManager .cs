using Core.Interfaces.Identity;
using Core.Results;

namespace Infrastructure.Persistence.Identity
{

    public class AppUserManager : IAppUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IQueryable<ApplicationUser> Users => (IQueryable<ApplicationUser>)_userManager.Users;

        IQueryable<IApplicationUser> IAppUserManager.Users => Users;

        public AppUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IApplicationUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IApplicationUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IList<string>> GetRolesAsync(IApplicationUser user)
        {
            return await _userManager.GetRolesAsync((ApplicationUser)user);
        }

        public async Task<bool> IsInRoleAsync(IApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync((ApplicationUser)user, role);
        }

        public async Task<IdentityResultDto> AddToRoleAsync(IApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync((ApplicationUser)user, role);
            return new IdentityResultDto() { Errors = result.Errors.Select(e => e.Description), Succeeded = result.Succeeded };
        }

        public async Task<IdentityResultDto> RemoveFromRoleAsync(IApplicationUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync((ApplicationUser)user, role);
            return new IdentityResultDto() { Errors = result.Errors.Select(e => e.Description), Succeeded = result.Succeeded };
        }
    }
}
