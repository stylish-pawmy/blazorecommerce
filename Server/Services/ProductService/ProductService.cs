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
            .Where(p => !p.Deleted && p.Visible)
            .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
            .ThenInclude(v => v.ProductType)
            .ToListAsync()
        };
        
        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        var response = new ServiceResponse<List<Product>> {
            Data = await _context.Products
            .Where(p => !p.Deleted && p.Visible)
            .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
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
        .Where(p => !p.Deleted && p.Visible)
        .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
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

    public async Task<ServiceResponse<List<string>>> GetProductsSearchSuggestionsAsync(string searchText)
    {
        var results = new List<string>();
        var searchResults = await GetProductsSearchListAsync(searchText);

        foreach (var product in searchResults)
        {
            //Title suggestions
            if (product.Title.ToLower().Contains(searchText.ToLower()))
                results.Add(product.Title);

            //Description suggestions
            if (product.Description is not null)
            {
                var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
                var words = product.Description.Split().Select(w => w.Trim(punctuation));

                foreach(var word in words)
                {
                    if (word.ToLower().Contains(searchText) && !results.Contains(word))
                        results.Add(word);
                }
            }
        }


        var response = new ServiceResponse<List<string>>() { Data = results};
        return response;
    }

    public async Task<ServiceResponse<ProductSearchResult>> SearchProductsAsync(string searchText, int page)
    {
        const float pageResults = 2f;
        var pagesNumber = Math.Ceiling((await GetProductsSearchListAsync(searchText)).Count / pageResults);
        var prodcuts = await _context.Products
                        .Where(p => 
                            p.Title.ToLower().Contains(searchText.ToLower())
                            || p.Description.ToLower().Contains(searchText.ToLower())
                        )
                        .Where(p => !p.Deleted && p.Visible)
                        .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
                        .ThenInclude(v => v.ProductType)
                        .Skip((page - 1) * (int) pageResults)
                        .Take((int) pageResults)
                        .ToListAsync();

        var response = new ServiceResponse<ProductSearchResult>()
        {
            Data = new ProductSearchResult() {
                Pages = (int) pagesNumber,
                CurrentPage = page,
                Products = prodcuts
            }
        };

        return response;
    }

    public async Task<List<Product>> GetProductsSearchListAsync(string searchText)
    {
        return 
            await _context.Products
            .Where(p => 
                p.Title.ToLower().Contains(searchText.ToLower())
                || p.Description.ToLower().Contains(searchText.ToLower())
            )
            .Where(p => !p.Deleted && p.Visible)
            .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
            .ThenInclude(v => v.ProductType)
            .ToListAsync();
    }

    public async Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync()
    {
        var response = new ServiceResponse<List<Product>>() {
            Data = await _context.Products
            .Where(p => p.Featured)
            .Where(p => !p.Deleted && p.Visible)
            .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
            .ThenInclude(v => v.ProductType)
            .ToListAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetAdminProductsAsync()
    {
        var response = new ServiceResponse<List<Product>>() {
            Data = await _context.Products
            .Where(p => !p.Deleted)
            .Include(p => p.Variants.Where(v => !v.Deleted && v.Visible))
            .ThenInclude(v => v.ProductType)
            .ToListAsync()
        };

        return response;
    }
}