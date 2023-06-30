namespace BlazorEcommerce.Server.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category)
    {
        category.Editing = category.IsNew = false;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return await GetAdminCategoriesAsync();
    }

    public async Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int categoryId)
    {
        var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        if (category is null)
            return new ServiceResponse<List<Category>>{
                Success = false,
                Message = "Category not found."
            };
        
        category.Deleted = true;
        await _context.SaveChangesAsync();
        return await GetAdminCategoriesAsync();
    }

    public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
    {
        var response = new ServiceResponse<List<Category>>()
        {
            Data = await _context.Categories.Where(c => !c.Deleted).ToListAsync<Category>()
        };

        return response;
    }

    public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
    {
        var response = new ServiceResponse<List<Category>>()
        {
            Data = await _context.Categories.Where(c => c.Visible && !c.Deleted).ToListAsync<Category>()
        };

        return response;
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public async Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category)
    {
        var dbCategory = await GetCategoryByIdAsync(category.Id);

        if (dbCategory is null)
            return new ServiceResponse<List<Category>> {
                Success = false,
                Message = "Category not found."
            };
        
        dbCategory.Name = category.Name;
        dbCategory.Url = category.Url;
        dbCategory.Visible = category.Visible;

        await _context.SaveChangesAsync();
        
        return await GetAdminCategoriesAsync();
    }
}