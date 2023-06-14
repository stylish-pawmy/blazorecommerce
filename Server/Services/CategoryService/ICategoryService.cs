namespace BlazorEcommerce.Server.Services.CategoryService;

public interface ICategoryService
{
    public Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
}