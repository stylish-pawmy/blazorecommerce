using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
        var response = await _productService.GetProductsAsync();
        return Ok(response);
    }

    [HttpGet("category/{categoryUrl}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
    {
        var response = await _productService.GetProductsByCategoryAsync(categoryUrl);
        return Ok(response);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
    {
        var response = await _productService.GetProductAsync(productId);
        return Ok(response);
    }

    [HttpGet("search/{searchText}/{page}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchProducts(string searchText, int page)
    {
        var response = await _productService.SearchProductsAsync(searchText, page);
        return Ok(response);
    }

    [HttpGet("searchsuggestions/{searchText}")]
    public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductsSearchSuggestions(string searchText)
    {
        var response = await _productService.GetProductsSearchSuggestionsAsync(searchText);
        return Ok(response);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
    {
        var response = await _productService.GetFeaturedProductsAsync();
        return Ok(response);
    }

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
    {
        var response = await _productService.GetAdminProductsAsync();
        return Ok(response);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
    {
        var response = await _productService.CreateProductAsync(product);
        return Ok(response);
    }
    
    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
    {
        var response = await _productService.UpdateProductAsync(product);
        return Ok(response);
    }

    [HttpDelete("{productId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
    {
        var response = await _productService.DeleteProductAsync(productId);
        return Ok(response);
    }
}