namespace BlazorEcommerce.Server.Services.ProductTypeService;

public class ProductTypeService : IProductTypeService
{
    private readonly ApplicationDbContext _context;

    public ProductTypeService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
    {
        var response = new ServiceResponse<List<ProductType>>();
        _context.ProductTypes.Add(productType);
        await _context.SaveChangesAsync();

        response.Data = (await GetProductTypes()).Data;

        return response;
    }

    public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
    {
        var response = new ServiceResponse<List<ProductType>>();
        var productTypes = await _context.ProductTypes.ToListAsync();

        response.Data = productTypes;

        return response;
    }

    public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
    {
        var response = new ServiceResponse<List<ProductType>>();
        var dbProductType = await _context.ProductTypes.Where(p => p.Id == productType.Id).FirstOrDefaultAsync();

        if (dbProductType is null)
        {
            response.Success = false;
            response.Message = "Product type not found.";
        }

        dbProductType.Name = productType.Name;
        await _context.SaveChangesAsync();

        response.Data = (await GetProductTypes()).Data;
        return response;
    }
}