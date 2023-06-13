namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }
    public List<Product> Products { get; set; } = new List<Product>();

    public async Task GetProducts()
    {
        var response =
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
        
        if (response is not null && response.Data is not null)
            Products = response.Data;
    }
}