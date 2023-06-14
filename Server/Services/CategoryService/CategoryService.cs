namespace BlazorEcommerce.Server.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
    {
        var response = new ServiceResponse<List<Category>>()
        {
            Data = await _context.Categories.ToListAsync<Category>()
        };

        return response;
    }
}