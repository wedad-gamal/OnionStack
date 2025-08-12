using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Logging;
using Infrastructure.Extensions;

namespace Infrastructure.Persistence.Identity
{
    public class AppUserManager : IAppUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggerManager _loggerManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AppUserManager(UserManager<ApplicationUser> userManager, ILoggerManager loggerManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _loggerManager = loggerManager;
            _signInManager = signInManager;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            _loggerManager.Info("Attempting to find user by ID: {UserId}", userId);
            var applicationUser = await _userManager.FindByIdAsync(userId);
            return applicationUser?.Adapt<UserDto>();
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            _loggerManager.Info("Attempting to find user by username: {UserName}", userName);
            var applicationUser = await _userManager.FindByNameAsync(userName);
            return applicationUser?.Adapt<UserDto>();
        }

        public async Task<IList<string>> GetUserRolesAsync(UserDto user)
        {
            _loggerManager.Info("Getting roles for user: {UserName}", user.UserName);
            var applicationUser = await _userManager.FindByIdAsync(user.Id);
            return applicationUser == null ? new List<string>() : await _userManager.GetRolesAsync(applicationUser);
        }

        public async Task<bool> IsInRoleAsync(UserDto user, string role)
        {
            _loggerManager.Info("Checking if user {UserName} is in role {Role}", user.UserName, role);
            var applicationUser = await _userManager.FindByIdAsync(user.Id);
            return applicationUser != null && await _userManager.IsInRoleAsync(applicationUser, role);
        }

        public async Task<IdentityResultDto> AssignRoleToUserAsync(UserDto user, string role)
        {
            _loggerManager.Info("Adding user {UserName} to role {Role}", user.UserName, role);
            var applicationUser = await _userManager.FindByIdAsync(user.Id);
            if (applicationUser == null)
            {
                _loggerManager.Warn("User not found for role assignment.");
                return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
            }
            var result = await _userManager.AddToRoleAsync(applicationUser, role);
            LogIdentityResult(result, $"add user {user.UserName} to role {role}");
            return result.ToDto();
        }
        public async Task<IdentityResultDto> AssignRoleToUserAsync(string userId, string role)
        {
            _loggerManager.Info("Adding user {UserId} to role {Role}", userId, role);
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null)
            {
                _loggerManager.Warn("User not found for role assignment.");
                return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
            }
            var result = await _userManager.AddToRoleAsync(applicationUser, role);
            LogIdentityResult(result, $"add user {userId} to role {role}");
            return result.ToDto();
        }

        public async Task<IdentityResultDto> RemoveRoleFromUserAsync(UserDto user, string role)
        {
            _loggerManager.Info("Removing user {UserName} from role {Role}", user.UserName, role);
            var applicationUser = await _userManager.FindByIdAsync(user.Id);
            if (applicationUser == null)
            {
                _loggerManager.Warn("User not found for role removal.");
                return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
            }
            var result = await _userManager.RemoveFromRoleAsync(applicationUser, role);
            LogIdentityResult(result, $"remove user {user.UserName} from role {role}");
            return result.ToDto();
        }

        public async Task<IdentityResultDto> CreateUserAsync(CreateUserDto userDto)
        {
            _loggerManager.Info("Creating user with email: {Email}", userDto.Email);
            var applicationUser = userDto.Adapt<ApplicationUser>();
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (await EmailExistsAsync(userDto.Email))
            {
                _loggerManager.Warn("User with email {Email} already exists.", userDto.Email);
                return Failure($"User with email {userDto.Email} already exists.");
            }

            if (userDto.Password != userDto.ConfirmPassword)
            {
                return new IdentityResultDto()
                {
                    Errors = new List<string> { "Password and Confirm Password do not match" },
                    Succeeded = false
                };
            }
            var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
            LogIdentityResult(result, $"create user {userDto.Email}");
            return result.ToDto();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            _loggerManager.Info("Retrieving all users.");
            return await _userManager.Users.Select(user => user.Adapt<UserDto>()).ToListAsync();
        }

        public async Task<IdentityResultDto> UpdateUserAsync(UserDto userDto)
        {
            _loggerManager.Info("Updating user with ID: {UserId}", userDto.Id);
            var applicationUser = await _userManager.FindByIdAsync(userDto.Id);
            if (applicationUser == null)
            {
                _loggerManager.Warn("User not found for update.");
                return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
            }

            applicationUser.Email = userDto.Email;
            applicationUser.PhoneNumber = userDto.PhoneNumber;
            applicationUser.UserName = userDto.UserName;
            applicationUser.FirstName = userDto.FirstName;
            applicationUser.LastName = userDto.LastName;
            applicationUser.Age = userDto.Age;

            var result = await _userManager.UpdateAsync(applicationUser);
            LogIdentityResult(result, $"update user {userDto.Id}");
            return result.ToDto();
        }

        public async Task<IdentityResultDto> DeleteUserAsync(string id)
        {
            _loggerManager.Info("Attempting to delete user with ID: {UserId}", id);
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                _loggerManager.Warn("User not found for deletion.");
                return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
            }
            var result = await _userManager.DeleteAsync(applicationUser);
            LogIdentityResult(result, $"delete user {id}");
            return result.ToDto();
        }

        public async Task<List<RoleDto>> GetUserRolesDtoAsync(string userId)
        {
            _loggerManager.Info("Getting roles for user with ID: {UserId}", userId);
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null) return new();

            var roles = await _userManager.GetRolesAsync(applicationUser);
            return roles.Select(r => new RoleDto { Name = r }).ToList();
        }
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                _loggerManager.Info("Generating password reset token for email: {Email}", email);
                var applicationUser = await _userManager.FindByEmailAsync(email);
                if (applicationUser == null) return string.Empty;

                var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
                _loggerManager.Info("Generated password reset token for email: {Email}", email);
                return token;


            }
            catch (Exception ex)
            {
                _loggerManager.Error("An Exception has occured , {ex}", ex);
                throw;
            }
        }
        public async Task<IdentityResultDto> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                _loggerManager.Info("Resetting password for email: {Email}", email);
                var applicationUser = await _userManager.FindByEmailAsync(email);
                if (applicationUser == null)
                {
                    _loggerManager.Warn("User not found for password reset.");
                    return new IdentityResultDto { Succeeded = false, Errors = new[] { "User not found" } };
                }
                var result = await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);
                LogIdentityResult(result, $"reset password for user {email}");
                return result.ToDto();

            }
            catch (Exception ex)
            {
                _loggerManager.Error("An error has occured, {ex}", ex);
                throw;
            }
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetUserByUserEmailAsync(string email)
        {
            _loggerManager.Info("Attempting to find user by email: {Email}", email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _loggerManager.Warn("User not found for email: {Email}", email);
                return null;
            }
            return user.Adapt<UserDto>();
        }
        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            _loggerManager.Info("Checking password for email: {Email}", email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _loggerManager.Warn("User not found for email: {Email}", email);
                return false;
            }
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResultDto> LoginAsync(LoginDto loginDto)
        {
            _loggerManager.Info("Attempting login for: {Email}", loginDto.Email);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new IdentityResultDto
                {
                    Succeeded = false,
                    Errors = new List<string> { "Invalid email or password" }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);

            return new IdentityResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Succeeded ? null : new List<string> { "Login failed" }
            };
        }

        private void LogIdentityResult(IdentityResult result, string action)
        {
            if (result.Succeeded)
            {
                _loggerManager.Info("Successfully completed action: {Action}.", action);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _loggerManager.Warn("Failed to {Action}: {Error}", action, error.Description);
                }
            }
        }

        async Task<bool> EmailExistsAsync(string email)
     => await _userManager.FindByEmailAsync(email) is not null;

        IdentityResultDto Failure(string error)
            => new IdentityResultDto
            {
                Succeeded = false,
                Errors = new List<string> { error }
            };

    }
}
