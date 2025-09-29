

namespace Infrastructure.Services.Features;
internal class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
        return categories.Adapt<List<CategoryDto>>();
    }

    public Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryDto> ToggleStatus(int id)
    {
        var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
        category.IsDeleted = !category.IsDeleted;
        category.ModifiedOn = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return category.Adapt<CategoryDto>();
    }

    public Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }
}
