namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    private readonly HttpClient _http;
    public event Action ProductListChanged = () => {};
    public List<Product>? Products { get; set; } = null;
    public List<Product>? AdminProducts { get; set; } = null;
    public string Message { get; set; } = string.Empty;
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;
    public string LastSearchText { get; set; } = string.Empty;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task GetProducts(string? categoryUrl = null)
    {
        var response = categoryUrl == null ?
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured"):
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
        
        if (response is not null && response.Data is not null)
            Products = response.Data;

        CurrentPage = 1;
        PageCount = 0;

        if (Products.Count == 0)
            Message = "No products were found.";
        
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

    public async Task SearchProducts(string searchText, int page)
    {
        LastSearchText = searchText;

        var response = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");
        if (response is not null && response.Data is not null)
        {
            Products = response.Data.Products;
            CurrentPage = response.Data.CurrentPage;
            PageCount = response.Data.Pages;
        }
        if (Products.Count == 0) Message = "No Products found.";
        ProductListChanged.Invoke();
    }

    public async Task<List<string>> GetProductsSearchSuggestions(string searchText)
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
        return response.Data;
    }

    public async Task GetAdminProducts()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
        AdminProducts = response.Data;
        
        CurrentPage = 1;
        PageCount = 0;

        if (AdminProducts.Count == 0)
            Message = "No products found.";

    }

    public async Task DeleteProduct(Product product)
    {
        var response = await _http.DeleteFromJsonAsync<ServiceResponse<bool>>($"api/product/{product.Id}");        
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var response = await _http.PostAsJsonAsync<Product>("api/product", product);
        var newProduct = (await response.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

        return newProduct;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var response = await _http.PutAsJsonAsync<Product>("api/product", product);
        var updatedProduct = (await response.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

        return updatedProduct;
    }
}