using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService;
public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authState;

    public CartService(
        ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authState)
    {
        _localStorage = localStorage;
        _http = http;
        _authState = authState;
    }

    public event Action OnChange = () => {};

    private async Task<bool> IsUserAuthenticated() =>
        (await _authState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;

    public async Task AddToCart(CartItem cartItem)
    {
        if (await IsUserAuthenticated())
        {
            var result = await _http.PostAsJsonAsync<CartItem>("api/cart/add", cartItem);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart is null) cart = new List<CartItem>();

            var itemInCart = cart.Find(i => i.ProductId == cartItem.ProductId && i.ProductTypeId == cartItem.ProductTypeId);
            if (itemInCart is null)
            {
                cart.Add(cartItem);
            }
            else {
                itemInCart.Quantity += cartItem.Quantity;
            }

            await _localStorage.SetItemAsync<List<CartItem>>("cart", cart);
        }

        await GetCartCount();
        OnChange.Invoke();
    }


    public async Task<List<CartProductResponse>> GetCartProducts()
    {
        if (await IsUserAuthenticated())
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("/api/cart");
            return response.Data;
        }
        else 
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cartItems is null || cartItems.Count == 0)
                return new List<CartProductResponse>();

            var response = await _http.PostAsJsonAsync<List<CartItem>>("api/cart/products", cartItems);
            var cartProducts = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>()).Data;
            return cartProducts;
        }
    }

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        if (await IsUserAuthenticated())
        {
            await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
        }
        else {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            var cartItem = cartItems.Find(i => i.ProductId == productId && i.ProductTypeId == productTypeId);

            if (cartItem is null) return;

            if (cartItem is not null)
            {
                cartItems.Remove(cartItem);
                await _localStorage.SetItemAsync<List<CartItem>>("cart", cartItems);
                await GetCartCount();
            }
        }
    }

    public async Task StoreCartItems(bool emptyLocalCart)
    {
        var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
        if (localCart is null) return;

        var result = await _http.PostAsJsonAsync<List<CartItem>>("api/cart", localCart);

        if (emptyLocalCart)
            await _localStorage.RemoveItemAsync("cart");
    }

    public async Task UpdateProductQuantity(CartProductResponse product)
    {
        if (await IsUserAuthenticated())
        {
            var request = new CartItem
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                ProductTypeId = product.ProductTypeId
            };

            await _http.PutAsJsonAsync<CartItem>("api/cart/update-quantity", request);
        }
        else {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");

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

    public async Task GetCartCount()
    {
        int count;
        if (await IsUserAuthenticated())
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
            count = result.Data;
        }
        else {
            var cart = (await _localStorage.GetItemAsync<List<CartItem>>("cart"));
            count = (cart is null) ? 0 : cart.Count;
        }

        await _localStorage.SetItemAsync<int>("cartItemsCount", count);
        OnChange.Invoke();
    }
}