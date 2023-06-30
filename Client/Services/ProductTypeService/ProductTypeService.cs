namespace BlazorEcommerce.Client.Services.ProductTypeService;

public class ProductTypeService : IProductTypeService
{
    private readonly HttpClient _http;
    public event Action OnChange = () => {};
    public List<ProductType> ProductTypes { get; set; }

    public ProductTypeService(HttpClient http)
    {
        this._http = http;
    }

    public async Task GetProductTypes()
    {
        var result = await _http
            .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");

        if (result is not null && result.Data is not null)
            ProductTypes = result.Data;
        else {
            ProductTypes = new List<ProductType>();
        }
        
        OnChange.Invoke();
    }

    public async Task AddProductType(ProductType productType)
    {
        var response = await _http.PostAsJsonAsync<ProductType>("api/producttype", productType);
        ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
        OnChange.Invoke();
    }

    public async Task UpdateProductType(ProductType productType)
    {
        var response = await _http.PutAsJsonAsync<ProductType>("api/producttype", productType);
        ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
        OnChange.Invoke();
    }

    public ProductType CreateProductType()
    {
        var productType = new ProductType {IsNew = true, Editing = true};
        ProductTypes.Add(productType);
        OnChange.Invoke();

        return productType;
    }
}