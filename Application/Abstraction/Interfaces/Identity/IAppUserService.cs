namespace Abstraction.Abstraction.Interfaces.Identity
{
    public interface IAppUserService
    {
        Task<IdentityResultDto> AssignRoleToUserAsync(UserDto user, string role);
        Task<IdentityResultDto> AssignRoleToUserAsync(string userId, string role);
        Task<IdentityResultDto> CreateUserAsync(CreateUserDto userDto);
        Task<IdentityResultDto> DeleteUserAsync(string id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByUserNameAsync(string userName);
        Task<UserDto> GetUserByUserEmailAsync(string email);
        Task<IdentityResultDto> LoginAsync(LoginDto loginDto);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<IList<string>> GetUserRolesAsync(UserDto user);
        Task<List<RoleDto>> GetUserRolesDtoAsync(string userId);
        Task<bool> IsInRoleAsync(UserDto user, string role);
        Task<IdentityResultDto> RemoveRoleFromUserAsync(UserDto user, string role);
        Task<IdentityResultDto> UpdateUserAsync(UserDto userDto);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<IdentityResultDto> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(string email);
    }
}
