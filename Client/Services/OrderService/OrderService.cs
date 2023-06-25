using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    public OrderService(
        HttpClient http,
        AuthenticationStateProvider authStateProvider,
        NavigationManager navigationManager
    )
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
    }

    public async Task PlaceOrder()
    {
        if (await IsUserAuthenticated())
        {
            var result = await _http.PostAsync("api/order", null);
        }
        else {
            _navigationManager.NavigateTo("login");
        }
    }

    public async Task<bool> IsUserAuthenticated()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task<List<OrderOverviewResponse>> GetOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");
        return result.Data;
    }
}