using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.ProductService;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
    {
        var response = new ServiceResponse<List<Product>> {
            Data = await _context.Products
            .Include(p => p.Variants)
            .ThenInclude(v => v.ProductType)
            .ToListAsync()
        };
        
        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        var response = new ServiceResponse<List<Product>> {
            Data = await _context.Products
            .Include(p => p.Variants)
            .ThenInclude(v => v.ProductType)
            .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
            .ToListAsync()
        };
        
        return response;
    }

    public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
    {
        var response = new ServiceResponse<Product>();
        var product = await _context.Products
        .Include(p => p.Variants)
        .ThenInclude(v => v.ProductType)
        .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            response.Message = "Product could not be retrieved from Database.";
            response.Success = false;
        }
        else
        {
            response.Data = product;
            response.Message = "Product retrieved successfully.";
        }

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText)
    {
        var response = new ServiceResponse<List<Product>>()
        {
            Data = await _context.Products
            .Where(p => 
                p.Title.ToLower().Contains(searchText.ToLower())
                || p.Description.ToLower().Contains(searchText.ToLower())
            )
            .Include(p => p.Variants)
            .ThenInclude(v => v.ProductType)
            .ToListAsync()
        };

        return response;
    }
}