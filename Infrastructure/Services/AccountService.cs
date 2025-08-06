namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAppUserManager _appUserManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUrlGenerator _urlGenerator;
        private readonly ILoggerManager _loggerManager;

        public AccountService(IAppUserManager appUserManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, IUrlGenerator urlGenerator, ILoggerManager loggerManager)
        {
            _appUserManager = appUserManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _urlGenerator = urlGenerator;
            _loggerManager = loggerManager;
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
    }
}
