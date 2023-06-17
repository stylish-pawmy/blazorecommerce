using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService;
public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public CartService(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public event Action OnChange = () => {};

    public async Task AddToCart(CartItem cartItem)
    {
        var cart = await GetCartItems();

        cart.Add(cartItem);

        await _localStorage.SetItemAsync<List<CartItem>>("cart", cart);

        OnChange.Invoke();
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
        if (cart is null) cart = new List<CartItem>();

        return cart;
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts()
    {
        var cartItems = await GetCartItems();
        var response = await _http.PostAsJsonAsync<List<CartItem>>("api/cart/products", cartItems);

        var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

        return cartProducts;
    }
}