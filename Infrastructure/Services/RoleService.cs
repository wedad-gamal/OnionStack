namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerManager _logger;
        private readonly IAppUserManager _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, ILoggerManager logger, IAppUserManager userManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IEnumerable<RoleDto>> GetAsync()
        {
            var roles = await _roleManager.Roles
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .ToListAsync();

            return roles;
        }

        public async Task<RoleDto> GetByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<RoleDto> GetByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<IdentityResultDto> CreateRoleAsync(string roleName)
        {
            _logger.Info("Attempting to create role: {RoleName}", roleName);

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return new IdentityResultDto
                {
                    Succeeded = false,
                    Errors = new[] { "Role name cannot be empty." }
                };
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    _logger.Warn("Failed to create role {RoleName}: {Error}", roleName, error.Description);
            }

            return new IdentityResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<IdentityResultDto> UpdateRoleAsync(RoleDto roleDto)
        {
            _logger.Info("Updating role: {RoleName}", roleDto.Name);

            var role = await _roleManager.FindByIdAsync(roleDto.Id);
            if (role == null)
            {
                _logger.Warn("Role with ID {RoleId} not found for update.", roleDto.Id);
                return new IdentityResultDto
                {
                    Succeeded = false,
                    Errors = new[] { "Role not found." }
                };
            }

            role.Name = roleDto.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    _logger.Warn("Failed to update role {RoleName}: {Error}", role.Name, error.Description);
            }

            return new IdentityResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<IdentityResultDto> RemoveRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                _logger.Warn("Role {RoleName} not found for deletion.", roleName);
                return new IdentityResultDto
                {
                    Succeeded = false,
                    Errors = new[] { "Role not found." }
                };
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    _logger.Warn("Failed to delete role {RoleName}: {Error}", roleName, error.Description);
            }

            return new IdentityResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task AddUsersToRoleAsync(string roleName, List<RoleDto> users)
        {
            _logger.Info("Adding/removing users for role: {RoleName}", roleName);

            try
            {
                foreach (var userDto in users)
                {
                    var user = await _userManager.FindByIdAsync(userDto.Id);
                    if (user == null)
                    {
                        _logger.Warn("User with ID {UserId} not found.", userDto.Id);
                        continue;
                    }

                    var isInRole = await _userManager.IsInRoleAsync(user, roleName);
                    IdentityResultDto result = null;

                    if (isInRole && !userDto.IsAssigned)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                    else if (!isInRole && userDto.IsAssigned)
                    {
                        result = await _userManager.AddToRoleAsync(user, roleName);
                    }

                    if (result != null && !result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            _logger.Error("Error changing role for user {UserId}: {Error}",
                                null, user.Id, error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error in AddUsersToRoleAsync: {Exception}", ex, ex.ToString());
            }
        }
    }
}
