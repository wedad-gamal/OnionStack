


using Core.Interfaces.Identity;

namespace Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleService> _logger;
        private readonly IAppUserManager _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, ILogger<RoleService> logger, IAppUserManager userManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task AddUsersToRoleAsync(string roleName, List<RoleDto> users)
        {
            _logger.LogInformation($"Adding role {roleName} ");
            try
            {
                IdentityResultDto result = new();
                foreach (var userDto in users)
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == userDto.Id);
                    if (await _userManager.IsInRoleAsync(user, roleName) && !userDto.IsAssigned)
                        result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    else if (!await _userManager.IsInRoleAsync(user, roleName) && userDto.IsAssigned)
                        result = await _userManager.AddToRoleAsync(user, roleName);

                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

        }

        public async Task<IdentityResultDto> CreateRoleAsync(string roleName)
        {
            _logger.LogInformation("Attempting to create role: {RoleName}", roleName);

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
                foreach (var error in result.Errors)
                    _logger.LogWarning("Failed to create role {RoleName}: {Error}", roleName, error.Description);
            return new IdentityResultDto
            {
                Errors = result.Errors.Select(r => r.Description),
                Succeeded = result.Succeeded
            };
        }

        public IEnumerable<RoleDto> Get()
             => _roleManager.Roles.Select(role => new RoleDto
             {
                 Id = role.Id,
                 Name = role.Name
             }).ToList();


        public async Task<RoleDto> GetByIdAsync(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            return new RoleDto()
            {
                Id = result.Id,
                Name = result.Name
            };
        }


        public async Task<IdentityResultDto> RemoveRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                foreach (var error in result.Errors)
                    _logger.LogWarning("Failed to delete role {RoleName}: {Error}", roleName, error.Description);
            return new IdentityResultDto
            {
                Errors = result.Errors.Select(r => r.Description),
                Succeeded = result.Succeeded
            };
        }

        public async Task<IdentityResultDto> UpdateRoleAsync(RoleDto roleDto)
        {
            _logger.LogInformation("Updating role: {RoleName}", roleDto.Name);

            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == roleDto.Id);
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Failed to update role {RoleName}: {Error}", role.Name, error.Description);
                }
            }
            return new IdentityResultDto
            {
                Errors = result.Errors.Select(r => r.Description),
                Succeeded = result.Succeeded
            };
        }
    }
}
