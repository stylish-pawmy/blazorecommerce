using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        var response = await _categoryService.GetCategoriesAsync();
        return Ok(response);
    }

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Category>>> GetAdminCategories()
    {
        var response = await _categoryService.GetAdminCategoriesAsync();
        return Ok(response);
    }

    [HttpPost("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Category>>> AddCategory(Category category)
    {
        var response = await _categoryService.AddCategoryAsync(category);
        return Ok(response);
    }

    [HttpPut("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Category>>> UpdateCategory(Category category)
    {
        var response = await _categoryService.UpdateCategoryAsync(category);
        return Ok(response);
    }

    [HttpDelete("admin/{categoryId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Category>>> DeleteCategory(int categoryId)
    {
        var response = await _categoryService.DeleteCategoryAsync(categoryId);
        return Ok(response);
    }
}