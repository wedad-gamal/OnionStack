using Application.DTOs;
using Core.Results;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILoggerManager _loggerManager;

        public RoleController(IRoleService roleService, ILoggerManager loggerManager)
        {
            _roleService = roleService;
            _loggerManager = loggerManager;
        }
        public async Task<IActionResult> Index()
        {
            var rolesDtos = await _roleService.GetAsync();
            return View(rolesDtos);
        }

        public IActionResult Create()
        {
            return View(new CreateRoleDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _roleService.CreateRoleAsync(model.Name);
            return await HandleResult(result, model, nameof(Create));
        }

        public async Task<IActionResult> Details(string id) => await GetDataHandler(id);

        public async Task<IActionResult> Edit(string id) => await GetDataHandler(id);
        public async Task<IActionResult> ConfirmDelete(string id) => await GetDataHandler(id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleDto model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _roleService.UpdateRoleAsync(model);
                return await HandleResult(result, model, nameof(Edit));
            }
            catch (Exception ex)
            {
                _loggerManager.Error("An error occurred while updating the role with ID {RoleId}", ex, id);
                TempData["ErrorMessage"] = "An error occurred while updating the role. Please try again later.";
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

            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await _roleService.RemoveRoleAsync(role.Name);
            return await HandleResult(result, role, nameof(Delete));
        }
        // ========== Helper Methods ==========
        private async Task<IActionResult> GetDataHandler(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var data = await _roleService.GetByIdAsync(id);
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
