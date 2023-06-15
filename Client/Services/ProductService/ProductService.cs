namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    private readonly HttpClient _http;
    public event Action ProductListChanged = () => {};
    public List<Product> Products { get; set; } = new List<Product>();
    public string Message { get; set; } = string.Empty;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetProducts(string? categoryUrl = null)
    {
        var response = categoryUrl == null ?
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product"):
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
        
        if (response is not null && response.Data is not null)
            Products = response.Data;
        
        ProductListChanged.Invoke();
    }
    
    public async Task<ServiceResponse<Product>> GetProduct(int productId)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");

        if (response is not null && response.Data is not null)
        {
            return response;
        }
        
        return new ServiceResponse<Product>()
                {
                    Success = false,
                    Message = "Could not fetch product."
                };
    }

    public async Task SearchProducts(string searchText)
    {
        Message = "Loading Products...";

        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
        Products = response.Data;

        if (Products.Count == 0) Message = "No Products found.";
        ProductListChanged.Invoke();
    }

    public async Task<List<string>> GetProductsSearchSuggestions(string searchText)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
        return response.Data;
    }
}