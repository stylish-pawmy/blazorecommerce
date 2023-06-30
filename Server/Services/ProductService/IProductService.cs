namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    public Task<ServiceResponse<List<Product>>> GetProductsAsync();
    public Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
    public Task<ServiceResponse<Product>> GetProductAsync(int productId);
    public Task<ServiceResponse<ProductSearchResult>> SearchProductsAsync(string searchText, int page);
    public Task<ServiceResponse<List<string>>> GetProductsSearchSuggestionsAsync(string searchText);
    public Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync();
    public Task<ServiceResponse<List<Product>>> GetAdminProductsAsync();
    public Task<ServiceResponse<Product>> UpdateProductAsync(Product product);
    public Task<ServiceResponse<bool>> DeleteProductAsync(int productId);
    public Task<ServiceResponse<Product>> CreateProductAsync(Product product);
}