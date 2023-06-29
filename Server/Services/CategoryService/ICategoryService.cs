namespace BlazorEcommerce.Server.Services.CategoryService;

public interface ICategoryService
{
    public Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    public Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();
    public Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category);
    public Task<Category> GetCategoryByIdAsync(int categoryId);
    public Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int categoryId);
    public Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category);


}