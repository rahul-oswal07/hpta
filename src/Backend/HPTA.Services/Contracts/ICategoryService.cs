using HPTA.Common;
using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ICategoryService
{
    Task AddAsync(CategoryEditModel category);
    Task<bool> CheckNameAvailabilityAsync(int? id, string name);
    Task<List<ListItem>> ListAsItemsAsync();
    Task<List<CategoryModel>> ListCategoriesAsync();
    Task<CategoryEditModel> GetCategoryByIdAsync(int id);
    Task UpdateAsync(CategoryEditModel category);
    Task DeleteAsync(int id);
}
