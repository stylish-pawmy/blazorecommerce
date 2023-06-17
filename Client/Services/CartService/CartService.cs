using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService;
public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;

    public CartService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public event Action OnChange = () => {};

    public async Task AddToCart(CartItem cartItem)
    {
        var cart = await GetCartItems();

        cart.Add(cartItem);

        await _localStorage.SetItemAsync<List<CartItem>>("cart", cart);
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
        if (cart is null) cart = new List<CartItem>();

        return cart;
    }

}