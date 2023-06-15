namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
    public event Action ProductListChanged;
    public List<Product> Products { get; set; }
    public Task GetProducts(string? categoryUrl = null);
    public Task<ServiceResponse<Product>> GetProduct(int productId);
}