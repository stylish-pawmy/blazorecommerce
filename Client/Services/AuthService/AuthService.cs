namespace BlazorEcommerce.Client.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authState;

    public AuthService(HttpClient http, AuthenticationStateProvider authState)
    {
        _http = http;
        _authState = authState;
    }

    public async Task<bool> IsUserAuthenticated() =>
        (await _authState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;

    public async Task<ServiceResponse<bool>> ChangePassword(ChangePassword request)
    {
        var response = await _http.PostAsJsonAsync<string>("api/auth/change-password", request.Password);
        return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var response = await _http.PostAsJsonAsync<UserLogin>("api/auth/login", request);
        
        return await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<int>> Register(UserRegister request)
    {
        var response = await _http.PostAsJsonAsync<UserRegister>("api/auth/register", request);

        return await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }
}