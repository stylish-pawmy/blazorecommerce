namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    public Task<ServiceResponse<List<Product>>> GetProductsAsync();
    public Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
    public Task<ServiceResponse<Product>> GetProductAsync(int productId);
}