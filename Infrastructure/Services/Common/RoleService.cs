using Application.Abstraction.Interfaces.Common;
using Application.DTOs.Common;

namespace Infrastructure.Services.Common
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerManager _logger;
        private readonly IAppUserService _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, ILoggerManager logger, IAppUserService userService)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userService;
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

        public async Task<IdentityResultDto> CreateRoleAsync(RoleDto roleDto)
        {
            _logger.Info("Attempting to create role: {RoleName}", roleDto.Name);


            if (await _roleManager.RoleExistsAsync(roleDto.Name))
            {
                var identityResult = IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateRole",
                    Description = $"The role '{roleDto.Name}' already exists."
                });
                return identityResult.Adapt<IdentityResultDto>();
            }
            var identityRole = new IdentityRole
            {
                Name = roleDto.Name
            };
            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    _logger.Warn("Failed to create role {RoleName}: {Error}", roleDto.Name, error.Description);
            }

            roleDto.Id = identityRole.Id;
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

        public async Task<List<UserRoleResultDto>> AddUsersToRoleAsync(string roleName, IEnumerable<UserRoleDto> users)
        {
            _logger.Info("Adding/removing users for role: {RoleName}", roleName);
            List<UserRoleResultDto> userRoleResults = new List<UserRoleResultDto>();
            bool isRoleChanged = false;
            try
            {
                foreach (var userDto in users)
                {
                    isRoleChanged = false;
                    var user = await _userManager.GetUserByIdAsync(userDto.UserId);
                    if (user == null)
                    {
                        _logger.Warn("User with ID {UserId} not found.", userDto.UserId);
                        continue;
                    }

                    var isInRole = await _userManager.IsInRoleAsync(user, roleName);
                    IdentityResultDto result = null;

                    if (isInRole && !userDto.IsAssigned)
                    {
                        result = await _userManager.RemoveRoleFromUserAsync(user, roleName);
                        isRoleChanged = true;
                    }
                    else if (!isInRole && userDto.IsAssigned)
                    {
                        result = await _userManager.AssignRoleToUserAsync(user, roleName);
                        isRoleChanged = true;
                    }

                    if (result != null && !result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            _logger.Error("Error changing role for user {UserId}: {Error}",
                                null, user.Id, error);
                    }
                    if (isRoleChanged)
                        userRoleResults.Add(new UserRoleResultDto
                        {
                            Email = user.Email,
                            UserId = user.Id,
                            RoleName = roleName,
                            UserName = user.UserName,
                            IsAssigned = userDto.IsAssigned,
                            Succeed = result?.Succeeded
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error in AddUsersToRoleAsync: {Exception}", ex, ex.ToString());
            }
            return userRoleResults;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllAsync(string roleName)
        {
            _logger.Info("Get all user roles ");
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                _logger.Warn("Role {RoleName} not found.", roleName);
                return Enumerable.Empty<UserRoleDto>();
            }
            var users = await _userManager.GetAllUsersAsync();
            var userRoles = new List<UserRoleDto>();
            foreach (var user in users)
            {
                var isAssigned = await _userManager.IsInRoleAsync(user, roleName);
                userRoles.Add(new UserRoleDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    RoleName = roleName,
                    IsAssigned = isAssigned
                });

            }
            return userRoles;
        }

        public async Task<IEnumerable<UserRoleDto>> GetIsAssignedUsersAllAsync(string roleName)
        {
            var users = await _userManager.GetAllUsersAsync();
            var result = users.Select(u => new UserRoleDto()
            {
                UserId = u.Id,
                UserName = u.UserName,
                IsAssigned = _userManager.IsInRoleAsync(u, roleName).Result
            }).ToList();

            return result;
        }
    }
}
