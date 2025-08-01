namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ILoggerManager _logger;
    private readonly UserManager<IApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(ILoggerManager logger, UserManager<IApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<IdentityResultDto> AssignRoleToUserAsync(string userId, string roleName)
    {
        _logger.Info("Assigning role {RoleName} to user {UserId}", roleName, userId);

        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.Warn("Failed to assign role {RoleName} to user {UserId}: {Error}", roleName, userId, error.Description);
            }
            throw new InvalidOperationException($"Failed to assign role {roleName} to user {userId}");
        }
        return new IdentityResultDto
        {
            Errors = result.Errors.Select(r => r.Description),
            Succeeded = result.Succeeded
        };
    }

    public async Task DeleteUserAsync(IApplicationUser user)
    {
        var existingUser = await _userManager.FindByIdAsync(user.Id);
        await _userManager.DeleteAsync(existingUser);
    }

    public IEnumerable<IApplicationUser> GetAllUsers() => _userManager.Users.ToList();

    public async Task<IApplicationUser> GetUserByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);

    public async Task<IApplicationUser> GetUserByUserNameAsync(string userName) => await _userManager.FindByNameAsync(userName);

    public async Task<List<RoleDto>> GetUserRolesDtoAsync(string userId)
    {
        _logger.Info(userId, "Getting roles for user {UserId}", userId);

        var user = await _userManager.FindByIdAsync(userId);

        var userRoles = await _userManager.GetRolesAsync(user);

        var allRoles = _roleManager.Roles.ToList();

        var roles = allRoles.Select(role => new RoleDto()
        {
            Id = role.Id,
            Name = role.Name,
            IsAssigned = userRoles.Contains(role.Name)
        }).ToList();

        return roles;

    }

    public async Task<IdentityResultDto> RemoveRoleFromUserAsync(string userId, string roleName)
    {
        _logger.Info($"Remove Role from User {userId} - role {roleName}");

        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            foreach (var error in result.Errors)
            {
                _logger.Error($"Error to remove {roleName} from {user.Id} - {error.Description}");
            }

            return new IdentityResultDto
            {
                Errors = result.Errors.Select(r => r.Description),
                Succeeded = result.Succeeded
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
            throw;
        }
    }

    public async Task<IdentityResultDto> UpdateUserAsync(IApplicationUser user)
    {
        var existingUser = await _userManager.FindByIdAsync(user.Id);
        var result = await _userManager.UpdateAsync(existingUser);
        return new IdentityResultDto
        {
            Errors = result.Errors.Select(r => r.Description),
            Succeeded = result.Succeeded
        };

    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        _logger.Info(userId, "Getting roles for user {UserId}", userId);

        var user = await _userManager.FindByIdAsync(userId);

        var userRoles = await _userManager.GetRolesAsync(user);
        return userRoles;


    }
}