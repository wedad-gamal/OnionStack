namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _loggerManager;

        public UsersController(IServiceManager serviceManager, ILoggerManager loggerManager)
        {
            _serviceManager = serviceManager;
            _loggerManager = loggerManager;
        }
        public async Task<IActionResult> Index()
        {
            var usersDto = await _serviceManager.AppUserService.GetAllUsersAsync();
            return View(usersDto);
        }

        public async Task<IActionResult> Details(string id) => await GetDataHandler(id);

        public async Task<IActionResult> Edit(string id) => await GetDataHandler(id);
        public async Task<IActionResult> ConfirmDelete(string id) => await GetDataHandler(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserDto model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _serviceManager.AppUserService.UpdateUserAsync(model);
                return await HandleResult(result, model, nameof(Edit));
            }
            catch (Exception ex)
            {
                _loggerManager.Error("An error occurred while updating the user with ID {userId}", ex, id);
                TempData["ErrorMessage"] = "An error occurred while updating the user. Please try again later.";
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var user = await _serviceManager.AppUserService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _serviceManager.AppUserService.DeleteUserAsync(id);
            return await HandleResult(result, user, nameof(Delete));
        }
        // ========== Helper Methods ==========
        private async Task<IActionResult> GetDataHandler(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var data = await _serviceManager.AppUserService.GetUserByIdAsync(id);
            if (data == null) return NotFound();

            return View(data);
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
            return RedirectToAction(nameof(Index));
        }

    }
}
