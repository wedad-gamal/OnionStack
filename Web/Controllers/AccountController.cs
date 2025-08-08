using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoggerManager _loggerManager;
        private readonly IAccountService _accountService;

        public AccountController(ILoggerManager loggerManager, IAccountService accountService)
        {
            _loggerManager = loggerManager;
            _accountService = accountService;
        }

        public IActionResult Register()
        {
            return View(new CreateUserDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.RegisterAsync(model);
            if (!result.Succeeded)
                return await HandleResult(result, model);

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.LoginAsync(model);
            if (!result.Succeeded)
                return await HandleResult(result, model);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _accountService.ForgotPassword(model.Email, "ResetPassword", "Account");
            if (result.Succeeded)
                TempData["Success"] = "Check you email, reset link has sent.";
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _accountService.ResetPasswordAsync(model);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successfully.";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "Failed to reset password.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = "null")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //var properties = new AuthenticationProperties
            //{
            //    RedirectUri = Url.Action("ExternalLoginCallback"),
            //    Items =
            //    {
            //        { "LoginProvider", provider },
            //        { "ReturnUrl", returnUrl }
            //    }
            //};
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginWithGoogle()
        {
            //login with google
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                var claims = result.Principal.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
                var identity = new ClaimsIdentity(claims, "Google");
                var principal = new ClaimsPrincipal(identity);
                // Sign in the user with the claims
                var info = await _accountService.GetExternalLoginInfoAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var info = await _accountService.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _accountService.HandleExternalLoginAsync(info);

            if (result.Succeeded)
                return string.IsNullOrWhiteSpace(returnUrl) ? RedirectToLocal(returnUrl) : RedirectToAction("Index", "Home");

            return RedirectToAction(nameof(Login), new { ReturnUrl = returnUrl });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private async Task<IActionResult> HandleResult(IdentityResultDto result, IModelDto model, string actionName = "operation")
        {
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            TempData["Success"] = $"{actionName} completed successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
