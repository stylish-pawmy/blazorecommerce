namespace BlazorEcommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Category> AdminCategories { get; set; } = new List<Category>(); 
    public event Action OnChange = () => {};

    public async Task AddCategory(Category category)
    {
        var response = await _http.PostAsJsonAsync($"api/category/admin", category);
        AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

        OnChange.Invoke();
    }

    public async Task<Category> CreateNewCategory()
    {
        var category = new Category {IsNew = true, Editing = true};
        AdminCategories.Add(category);
        OnChange.Invoke();
        return category;
    }

    public async Task DeleteCategory(int categoryId)
    {
        var response = await _http.DeleteFromJsonAsync<ServiceResponse<List<Category>>>($"api/category/admin/{categoryId}");
        if (response is not null && response.Data is not null)
        {
            AdminCategories = response.Data;
        }

        OnChange.Invoke();
    }

    public async Task GetAdminCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
        if (response is not null && response.Data is not null)
        {
            AdminCategories = response.Data;
        }

        OnChange.Invoke();

    }

    public async Task GetCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
        if (response is not null && response.Data is not null)
        {
            Categories = response.Data;
        }

        OnChange.Invoke();
    }

    public async Task UpdateCategory(Category category)
    {
        var response = await _http.PutAsJsonAsync($"api/category/admin", category);
        AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

        OnChange.Invoke();
    }
}