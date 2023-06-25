namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
    public event Action OnChange;
    public Task AddToCart(CartItem cartItem);
    public Task<List<CartProductResponse>> GetCartProducts();
    public Task RemoveProductFromCart(int productId, int productTypeId);
    public Task UpdateProductQuantity(CartProductResponse product);
    public Task StoreCartItems(bool emptyLocalCart);
    public Task GetCartCount();
}