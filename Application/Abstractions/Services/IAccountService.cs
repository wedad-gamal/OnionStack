namespace Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<IdentityResultDto> LoginAsync(LoginDto loginDto);
        Task<IdentityResultDto> RegisterAsync(CreateUserDto createUserDto);
        Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto);
        Task<IdentityResultDto> ForgotPassword(string email, string action, string controller);
        Task Logout();
    }
}
