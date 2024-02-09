using HPTA.Common;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : BaseController
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAllCategories()
    {
        return Ok(await _categoryService.ListCategoriesAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCategory(CategoryEditModel category)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _categoryService.AddAsync(category);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory(CategoryEditModel category)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _categoryService.UpdateAsync(category);
        return Ok();
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(ListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListItems()
    {
        List<ListItem> categories = await _categoryService.ListAsItemsAsync();
        return Ok(categories);
    }

    [HttpGet("check-name-availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckNameAvailability(int? id, string name)
    {
        bool isAvailable = await _categoryService.CheckNameAvailabilityAsync(id, name);
        return Ok(new { isAvailable });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok();
    }
}
