namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
    public string Message { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public string LastSearchText { get; set; }
    public event Action ProductListChanged;
    public List<Product> Products { get; set; }
    public Task GetProducts(string? categoryUrl = null);
    public Task<ServiceResponse<Product>> GetProduct(int productId);
    public Task SearchProducts(string searchText, int page);
    public Task<List<string>> GetProductsSearchSuggestions(string searchText);

}