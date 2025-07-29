
using Application.DTOs;

namespace Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task<IdentityResultDto> LoginAsync(LoginDto loginDto);
        Task<IdentityResultDto> RegisterAsync(RegisterDto registerDto);
        Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto);
        Task<IdentityResultDto> ForgotPassword(string email);
        Task Logout();
    }
}
