using Application.Commands;
using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Services;
using MediatR;
using Web.ViewModels;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAppUserManager _appUserManager;
        private readonly ILoggerManager _loggerManager;
        private readonly IMediator _mediator;

        public RoleController(IRoleService roleService, IAppUserManager appUserManager, ILoggerManager loggerManager,
            IMediator mediator)
        {
            _roleService = roleService;
            _appUserManager = appUserManager;
            _loggerManager = loggerManager;
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var rolesDto = await _roleService.GetAsync();
            return View(rolesDto);
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

        [HttpGet("Role/AssignOrRemoveRoleToUser/{roleName}")]
        public async Task<IActionResult> AssignOrRemoveRoleToUser(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest();
            var role = await _roleService.GetByNameAsync(roleName);
            var users = await _roleService.GetIsAssignedUsersAllAsync(roleName);
            var model = new AssignRoleToUsersViewModel
            {
                RoleName = roleName,
                Users = users.Select(u => new UserRoleViewModel()
                {
                    Id = u.UserId,
                    Name = u.UserName,
                    IsAssigned = u.IsAssigned
                }).ToList()
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> HandleAssignRemoveRolesToUser(AssignRoleToUsersViewModel userRoleViewModel)
        {
            ChangeRoleCommand changeRoleCommand = new ChangeRoleCommand
            {
                RoleName = userRoleViewModel.RoleName,
                Users = userRoleViewModel.Users.Select(u => new UserRoleDto()
                {
                    UserId = u.Id,
                    RoleName = userRoleViewModel.RoleName,
                    UserName = u.Name,
                    IsAssigned = u.IsAssigned
                })
            };
            var result = await _mediator.Send(changeRoleCommand);
            return RedirectToAction(nameof(Index));
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
