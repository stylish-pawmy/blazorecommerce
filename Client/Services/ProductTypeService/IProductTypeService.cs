namespace BlazorEcommerce.Client.Services.ProductTypeService;

public interface IProductTypeService
{
    event Action OnChange;
    List<ProductType> ProductTypes { get; set; }
    Task GetProductTypes();
}