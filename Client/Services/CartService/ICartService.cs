namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
    public event Action OnChange;
    public Task<List<CartItem>> GetCartItems();
    public Task AddToCart(CartItem cartItem);
    public Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts();
    public Task RemoveProductFromCart(int productId, int productTypeId);
    public Task UpdateProductQuantity(CartProductResponse product);
}