

namespace Infrastructure.Services.Features;
internal class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<CategoryDto> CreateCategoryAsync(CreateEditCategortDto categoryDto)
    {
        var category = categoryDto.Adapt<Category>();
        category.CreatedOn = DateTime.UtcNow;
        await _unitOfWork.Repository<Category, int>().AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return category.Adapt<CategoryDto>();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var catgory = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
        if (catgory == null)
            return false;
        _unitOfWork.Repository<Category, int>().Delete(catgory);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
        return categories.Adapt<List<CategoryDto>>();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
        return category.Adapt<CategoryDto>();
    }

    public async Task<CategoryDto> ToggleStatus(int id)
    {
        var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
        category.IsDeleted = !category.IsDeleted;
        category.ModifiedOn = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return category.Adapt<CategoryDto>();
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, CreateEditCategortDto categoryDto)
    {
        if (id != categoryDto?.Id)
            throw new ArgumentException("Category ID mismatch");
        var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException("Category not found");
        category.Name = categoryDto.Name;
        category.ModifiedOn = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return category.Adapt<CategoryDto>();
    }
}
