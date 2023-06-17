namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
    public event Action OnChange;
    public Task<List<CartItem>> GetCartItems();
    public Task AddToCart(CartItem cartItem);
}