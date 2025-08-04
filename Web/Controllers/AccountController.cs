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

        public IActionResult Login()
        {
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
