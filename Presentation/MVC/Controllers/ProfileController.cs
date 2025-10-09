using Application.DTOs.Common;

namespace MVC.Controllers;

public class ProfileController : Controller
{
    private readonly IServiceManager _serviceManager;

    // Controller code to manage user profiles would go here
    //Update User Profile with validation and error handling

    public ProfileController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(UpdateUserDto model)
    {
        if (!ModelState.IsValid)
            return View(model);
        try
        {
            //await _appUserManager.UpdateUserAsync(model);
            return View(model);
        }
        catch (Exception ex)
        {
            return View(model);
        }
    }


}
