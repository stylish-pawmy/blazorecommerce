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

        var itemInCart = cart.Find(i => i.ProductId == cartItem.ProductId && i.ProductTypeId == cartItem.ProductTypeId);
        if (itemInCart is null)
        {
            cart.Add(cartItem);
        }
        else {
            itemInCart.Quantity += cartItem.Quantity;
        }

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

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        var cartItems = await GetCartItems();

        var cartItem = cartItems.Find(i => i.ProductId == productId && i.ProductTypeId == productTypeId);

        if (cartItem is null) return;

        if (cartItem is not null)
        {
            cartItems.Remove(cartItem);
            await _localStorage.SetItemAsync<List<CartItem>>("cart", cartItems);
            OnChange.Invoke();
        }
    }

    public async Task UpdateProductQuantity(CartProductResponse product)
    {
        var cartItems = await GetCartItems();

        var cartItem = cartItems
            .Find(i => i.ProductId == product.ProductId && i.ProductTypeId == product.ProductTypeId);

        if (cartItem is null) return;

        if (cartItem is not null)
        {
            cartItem.Quantity = product.Quantity;
            await _localStorage.SetItemAsync<List<CartItem>>("cart", cartItems);
        }
    }
}