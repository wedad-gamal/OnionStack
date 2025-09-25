using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using MVC.Apis.Attributes;
using MVC.Apis.Common;
using MVC.ViewModels;

namespace MVC.Apis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleApiController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IMediator _mediator;
    private readonly IAntiforgery _antiforgery;

    public RoleApiController(IRoleService roleService, IMediator mediator, IAntiforgery antiforgery)
    {
        _roleService = roleService;
        _mediator = mediator;
        _antiforgery = antiforgery;
    }


    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAsync();
        return Ok(ApiResponse<IEnumerable<RoleDto>>.Ok(roles, "Roles fetched successfully"));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto createRoleDto)
    {
        if (string.IsNullOrWhiteSpace(createRoleDto.Name))
        {
            return BadRequest(new { message = "Role name is required." });
        }
        var roleDto = new RoleDto { Name = createRoleDto.Name };
        var result = await _roleService.CreateRoleAsync(roleDto);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join("; ", result.Errors);
            return StatusCode(500, new { message = errorMessage });
        }

        return Ok(new { roleDto.Id, roleDto.Name });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(string id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return NotFound("Role not found");

        return Ok(new { role.Id, role.Name });
    }
    [HttpPut("UpdateRole/{id}")]
    [ValidateCsrfToken]
    public async Task<IActionResult> UpdateRole(string id, RoleDto model)
    {
        if (id != model.Id)
            return BadRequest(ApiResponse<RoleDto>.Fail("ID mismatch"));

        if (!ModelState.IsValid)
        {
            var message = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(ApiResponse<RoleDto>.Fail(message));
        }

        var result = await _roleService.UpdateRoleAsync(model);
        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.SelectMany(e => e));
            return BadRequest(ApiResponse<RoleDto>.Fail(errors));
        }
        return Ok(ApiResponse<object>.Ok(model, "Role Updated Successfully"));
    }

    [HttpDelete("{id}")]
    [ValidateCsrfToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Ok(ApiResponse<RoleDto>.Fail($"Id {id} not found"));

        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
            return Ok(ApiResponse<RoleDto>.Fail($"Id {id} not found"));

        var result = await _roleService.RemoveRoleAsync(role.Name);
        return Ok(ApiResponse<RoleDto>.Ok(role, "Role deleted successfully"));
    }


    [HttpPost("assignRole")]
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
        if (result)
            return Ok(ApiResponse<string>.Ok(null, "Role assignments updated successfully."));
        else
            return Ok(ApiResponse<string>.Fail("Failed to update role assignments."));
    }
}
