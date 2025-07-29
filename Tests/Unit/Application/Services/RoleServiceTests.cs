

using Application.Implementation;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Unit.Application.Services;

public class RoleServiceTests
{
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private readonly RoleService _roleService;

    //public RoleServiceTests()
    //{
    //    var store = new Mock<IRoleStore<IdentityRole>>();
    //    _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
    //        store.Object, null, null, null, null
    //    );

    //    _roleService = new RoleService(_mockRoleManager.Object);
    //}


    // Add your test methods here
    // Example:
    // [Fact]
    // public async Task CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
    // {
    //     var roleName = "TestRole";
    //     _mockRoleManager.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
    //         .ReturnsAsync(IdentityResult.Success);
    //
    //     var result = await _roleService.CreateRoleAsync(roleName);
    //
    //     Assert.True(result.Success);
    // }


}