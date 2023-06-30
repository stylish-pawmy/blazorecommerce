namespace BlazorEcommerce.Server.Services.ProductTypeService;

public class ProductTypeService : IProductTypeService
{
    private readonly ApplicationDbContext _context;

    public ProductTypeService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
    {
        var response = new ServiceResponse<List<ProductType>>();
        var productTypes = await _context.ProductTypes.ToListAsync();

        response.Data = productTypes;

        return response;
    }
}