

namespace MVC.Controllers;

public class CategoriesController : Controller
{
    private readonly IServiceManager _serviceManager;

    public CategoriesController(IServiceManager serviceManager)
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

    [HttpGet]
    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Form");
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEditCategortDto categoryDto)
    {
        if (!ModelState.IsValid)
            return PartialView("_Form", categoryDto);
        var newCategory = await _serviceManager.CategoryService.CreateCategoryAsync(categoryDto);
        return PartialView("_CategoryRow", newCategory);
    }

    [HttpGet]
    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);

        if (category is null) return NotFound();

        return PartialView("_Form", category.Adapt<CreateEditCategortDto>());
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, CreateEditCategortDto categoryDto)
    {
        if (!ModelState.IsValid)
            return PartialView("_Form", categoryDto);
        var newCategory = await _serviceManager.CategoryService.UpdateCategoryAsync(id, categoryDto);
        return PartialView("_CategoryRow", newCategory);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _serviceManager.CategoryService.DeleteCategoryAsync(id);
        return result ? Ok() : BadRequest();
    }
}
