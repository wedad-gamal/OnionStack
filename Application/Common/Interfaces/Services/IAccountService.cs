using Application.Abstractions;
using Application.Common.Interfaces.Identity;

namespace Application.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> AddLoginAsync(IApplicationUser user, ExternalLoginInfo info);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<IdentityResult> CreateUserAsync(IApplicationUser user, string password = null);
        Task<IdentityResultDto> ForgotPassword(string email, string action, string controller);
        Task<IdentityResultDto> LoginAsync(LoginDto loginDto);
        Task Logout();
        Task<IdentityResultDto> RegisterAsync(CreateUserDto createUserDto);
        Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto);
        Task SignInAsync(IApplicationUser user, bool isPersistent);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent = false);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<SignInResult> HandleExternalLoginAsync(ExternalLoginInfo info);
    }
}
