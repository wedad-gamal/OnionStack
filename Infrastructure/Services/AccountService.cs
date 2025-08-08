using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAppUserManager _appUserManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUrlGenerator _urlGenerator;
        private readonly ILoggerManager _loggerManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IAppUserManager appUserManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, IUrlGenerator urlGenerator, ILoggerManager loggerManager,
            UserManager<ApplicationUser> userManager)
        {
            _appUserManager = appUserManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _urlGenerator = urlGenerator;
            _loggerManager = loggerManager;
            _userManager = userManager;
        }
        public async Task<IdentityResultDto> ForgotPassword(string email, string action, string controller)
        {

            _loggerManager.Info("Attempting to send password reset email to: {Email}", email);
            var token = await _appUserManager.GeneratePasswordResetTokenAsync(email);
            var url = _urlGenerator.GenerateUrl(email, token, action, controller);
            await _emailService.SendEmailAsync(email, "Reset Password", url);

            return new IdentityResultDto()
            {
                Succeeded = true,
                Errors = new List<string>() { "Reset password email sent successfully" }
            };

        }

        public async Task<IdentityResultDto> LoginAsync(LoginDto loginDto)
        {
            _loggerManager.Info("Attempting to log in user: {Email}", loginDto.Email);
            return await _appUserManager.LoginAsync(loginDto);
        }

        public async Task Logout()
        {
            _loggerManager.Info("Attempting to log out user ");
            await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResultDto> RegisterAsync(CreateUserDto createUserDto)
        {
            _loggerManager.Info("Attempting to register user: {Email}", createUserDto.Email);
            return await _appUserManager.CreateUserAsync(createUserDto);
        }

        public async Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto)
        {
            _loggerManager.Info("Attempting to reset password for user: {Email}", changePasswordDto.Email);
            return await _appUserManager.ResetPasswordAsync(changePasswordDto.Email, changePasswordDto.Token, changePasswordDto.Password);
        }


        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<IdentityResult> CreateUserAsync(IApplicationUser user, string password = null)
        {
            var applicationUser = user as ApplicationUser;
            return password == null
                ? await _userManager.CreateAsync(applicationUser)
                : await _userManager.CreateAsync(applicationUser, password);
        }

        public async Task<IdentityResult> AddLoginAsync(IApplicationUser user, ExternalLoginInfo info)
        {
            var applicationUser = user as ApplicationUser;
            return await _userManager.AddLoginAsync(applicationUser, info);
        }

        public async Task SignInAsync(IApplicationUser user, bool isPersistent)
        {
            var applicationUser = user as ApplicationUser;
            await _signInManager.SignInAsync(applicationUser, isPersistent);


        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent = false)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            return await _signInManager.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public async Task<SignInResult> HandleExternalLoginAsync(ExternalLoginInfo info)
        {

            if (info == null)
            {
                _loggerManager.Warn("External login info is null.");
                return SignInResult.Failed;
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                _loggerManager.Info("External login succeeded for provider {Provider}", info.LoginProvider);
                return result;
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var profilePicture = info.Principal.FindFirstValue("image") ?? "";


            if (string.IsNullOrEmpty(email))
            {
                _loggerManager.Warn("Email claim not found in external login info.");
                return SignInResult.Failed;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    ProfilePictureUrl = profilePicture
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _loggerManager.Error("User creation failed: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return SignInResult.Failed;
                }
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                _loggerManager.Error("Adding external login failed: {Errors}", string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
                return SignInResult.Failed;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _loggerManager.Info("User signed in after linking external login.");
            return SignInResult.Success;
        }

    }
}
