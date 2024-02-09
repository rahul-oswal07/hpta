using HPTA.Common;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class SubCategoryController(ISubCategoryService subCategoryService) : BaseController
{
    private readonly ISubCategoryService _subCategoryService = subCategoryService;

    [HttpGet]
    [ProducesResponseType(typeof(List<SubCategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAllSubCategories()
    {
        return Ok(await _subCategoryService.ListSubCategoriesAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSubCategory(int id)
    {
        var result = await _subCategoryService.GetSubCategoryByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostSubCategory(SubCategoryEditModel subCategory)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _subCategoryService.AddAsync(subCategory);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubCategory(SubCategoryEditModel subCategory)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _subCategoryService.UpdateAsync(subCategory);
        return Ok();
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(ListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListItems()
    {
        List<ListItem> categories = await _subCategoryService.ListAsItemsAsync();
        return Ok(categories);
    }

    [HttpGet("check-name-availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckNameAvailability(int? id, string name)
    {
        bool isAvailable = await _subCategoryService.CheckNameAvailabilityAsync(id, name);
        return Ok(new { isAvailable });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubCategory(int id)
    {
        await _subCategoryService.DeleteAsync(id);
        return Ok();
    }
}
