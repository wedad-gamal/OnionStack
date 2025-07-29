
namespace Application.Implementation
{
    public class AccountServices : IAccountService
    {
        private readonly UserManager<IApplicationUser> _userManager;
        private readonly SignInManager<IApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IResetPasswordUrlGenerator _resetPasswordUrlGenerator;
        private readonly IApplicationUserFactory _applicationUserFactory;

        public AccountServices(UserManager<IApplicationUser> userManager,
            SignInManager<IApplicationUser> signInManager, IEmailService emailService,
            IResetPasswordUrlGenerator resetPasswordUrlGenerator, IApplicationUserFactory applicationUserFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _resetPasswordUrlGenerator = resetPasswordUrlGenerator;
            _applicationUserFactory = applicationUserFactory;
        }
        public async Task<IdentityResultDto> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new IdentityResultDto()
                {
                    Success = false,
                    Errors = new List<string>() { "User is not exists" }
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = _resetPasswordUrlGenerator.GenerateUrl(email, token);

            await _emailService.SendEmailAsync(email, "Reset Password", url);

            return new IdentityResultDto()
            {
                Success = true,
                Errors = new List<string>() { "Reset password email sent successfully" }
            };
        }

        public async Task<IdentityResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is not null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return new IdentityResultDto { Success = true };
                }
            }
            return new IdentityResultDto { Success = false, Errors = new List<string> { "Invalid UserName or Password" } };
        }

        public async Task Logout()
           => await _signInManager.SignOutAsync();


        public async Task<IdentityResultDto> RegisterAsync(RegisterDto registerDto)
        {
            IApplicationUser applicationUser = _applicationUserFactory.Create();
            applicationUser.FirstName = registerDto.FirstName;
            applicationUser.LastName = registerDto.LastName;
            applicationUser.Email = registerDto.Email;
            applicationUser.UserName = registerDto.UserName;



            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);

            return new IdentityResultDto()
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };

        }

        public async Task<IdentityResultDto> ResetPasswordAsync(ResetPasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordDto.Email);
            if (user is null)
            {
                return new IdentityResultDto()
                {
                    Success = false,
                    Errors = new List<string>() { "User is not exists" }
                };
            }
            await _userManager.ResetPasswordAsync(user, changePasswordDto.Token, changePasswordDto.ConfirmPassword);
            return new IdentityResultDto()
            {
                Success = true,
                Errors = new List<string>() { "Password reset successfully" }
            };
        }
    }
}
