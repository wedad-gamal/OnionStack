namespace MVC.Controllers;

public class CategoryController : Controller
{
    private readonly IServiceManager _serviceManager;

    public CategoryController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var category = await _serviceManager.CategoryService.ToggleStatus(id);
        return Ok(category);
    }
}
