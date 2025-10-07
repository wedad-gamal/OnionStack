using Application.DTOs.Features;

namespace Application.Abstraction.Interfaces.Features;
public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateEditCategortDto categoryDto);
    Task<CategoryDto> UpdateCategoryAsync(int id, CreateEditCategortDto categoryDto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<CategoryDto> ToggleStatus(int id);
}
