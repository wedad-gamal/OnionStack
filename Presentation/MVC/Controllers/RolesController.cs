using Application.DTOs.Common;

namespace Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _loggerManager;

        public RolesController(IServiceManager serviceManager, ILoggerManager loggerManager)
        {
            _serviceManager = serviceManager;
            _loggerManager = loggerManager;
        }
        public async Task<IActionResult> Index()
        {
            var rolesDto = await _serviceManager.RoleService.GetAsync();
            return View(rolesDto);
        }
        public async Task<IActionResult> RolesTable()
        {
            var rolesDto = await _serviceManager.RoleService.GetAsync();
            return PartialView("_RolesTablePartialView", rolesDto);
        }
        public IActionResult Create()
        {
            return View(new CreateRoleDto());
        }
        public async Task<IActionResult> Details(string id) => await GetDataHandler(id);

        public async Task<IActionResult> Edit(string id) => await GetDataHandler(id);
        [HttpGet]
        public async Task<IActionResult> GetEditModal(string id)
        {
            var role = await _serviceManager.RoleService.GetByIdAsync(id);
            if (role == null)
            {
                _loggerManager.Warn("Role with id {Id} not found", id);
                return NotFound();
            }


            return PartialView("_EditRoleModal", role.Adapt<RoleDto>());
        }
        [HttpGet]
        public async Task<IActionResult> GetConfirmDeleteModal(string id)
        {
            var role = await _serviceManager.RoleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            return PartialView("_ConfirmDeleteRoleModal", role.Adapt<RoleDto>());
        }

        public async Task<IActionResult> ConfirmDelete(string id) => await GetDataHandler(id);
        public async Task<IActionResult> AssignOrRemoveRoleToUser(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest();
            var users = await _serviceManager.RoleService.GetIsAssignedUsersAllAsync(roleName);
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


        // ========== Helper Methods ==========
        private async Task<IActionResult> GetDataHandler(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var data = await _serviceManager.RoleService.GetByIdAsync(id);
            if (data == null) return NotFound();

            return View(data);
        }
    }
}
