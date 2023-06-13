using Microsoft.AspNetCore.Mvc;
using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> Get()
    {
        var products = await _context.Products.ToListAsync();
        var response = new ServiceResponse<List<Product>> {Data = products};
        return Ok(response);
    }
}