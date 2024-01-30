using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
}
