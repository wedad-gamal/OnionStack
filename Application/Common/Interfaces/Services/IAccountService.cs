using Application.Common.Interfaces.Identity;
using Application.DTOs.Identity;

namespace Application.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IdentityResultDto> ForgotPassword(string email, string action, string controller);
        Task<IdentityResultDto> LoginAsync(LoginDto loginDto);
        Task Logout();
        Task<IdentityResultDto> RegisterAsync(CreateUserDto createUserDto);
        Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto);
        AuthenticationPropertiesDto ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<IdentityResultDto> CreateUserAsync(IApplicationUser user, string password = null);
        Task<IdentityResultDto> AddLoginAsync(IApplicationUser user, ExternalLoginInfoDto info);
        Task SignInAsync(IApplicationUser user, bool isPersistent);
        Task<SignInResultDto> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent = false);
        Task<ExternalLoginInfoDto> GetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<SignInResultDto> HandleExternalLoginAsync(ExternalLoginInfoDto info);
    }
}
