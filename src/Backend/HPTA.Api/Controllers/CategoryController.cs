using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
}
