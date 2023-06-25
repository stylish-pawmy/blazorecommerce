using System.Security.Claims;
namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
    {
        var response = new ServiceResponse<List<CartProductResponse>>() 
        {
            Data = new List<CartProductResponse>()
        };

        foreach (var item in cartItems)
        {
            var product = await _context.Products
            .SingleOrDefaultAsync(p => p.Id == item.ProductId);
            if (product == null) continue;

            var variant = await _context.ProductVariants
            .Include(v => v.ProductType)
            .SingleOrDefaultAsync(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId);
            if (variant == null) continue;

            var productResponse = new CartProductResponse()
            {
                ProductId = product.Id,
                Title = product.Title,
                ImageUrl = product.ImageUrl,
                Price = variant.Price,
                ProductType = (variant.ProductType is null) ? string.Empty : variant.ProductType.Name,
                ProductTypeId = variant.ProductTypeId,
                Quantity = item.Quantity
            };

            response.Data.Add(productResponse);
        }

        return response;
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartProducts(List<CartItem> cartItems)
    {
        cartItems.ForEach(c => c.UserId = GetUserId());
        foreach (var item in cartItems)
        {
            await AddToCart(item);
        }

        return await GetDbCartProducts();
    }

    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()).Count;
        
        return new ServiceResponse<int>
        {
            Data = count
        };
    }

    public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
    {
        var result = await GetCartProducts(
            await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()
        );

        return result;
    }

    public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
    {
        var userId = GetUserId();

        cartItem.UserId = userId;

        var item = await _context.CartItems.FirstOrDefaultAsync(ci =>
            ci.ProductId == cartItem.ProductId
         && ci.ProductTypeId == cartItem.ProductTypeId
         && ci.UserId == cartItem.UserId 
        );

        if (item is null)
            _context.CartItems.Add(cartItem);
        else
            item.Quantity += cartItem.Quantity;
        
        await _context.SaveChangesAsync();
        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> UpdateProductQuantity(CartItem cartItem)
    {
        var item = await _context.CartItems.FirstOrDefaultAsync(ci =>
            ci.ProductId == cartItem.ProductId
         && ci.ProductTypeId == cartItem.ProductTypeId
         && ci.UserId == GetUserId() 
        );

        if (item is null)
            return new ServiceResponse<bool> {
                Data = false,
                Success = false,
                Message = "Product not found"
            };

        item.Quantity = cartItem.Quantity;
        await _context.SaveChangesAsync();
        
        return new ServiceResponse<bool> { Data = true };
    }

    public async Task<ServiceResponse<bool>> RemoveProduct(int productId, int productTypeId)
    {
        var item = await _context.CartItems.FirstOrDefaultAsync(ci =>
            ci.ProductId == productId
         && ci.ProductTypeId == productTypeId
         && ci.UserId == GetUserId() 
        );

        if (item is null)
            return new ServiceResponse<bool> {
                Data = false,
                Success = false,
                Message = "Product not found"
            };
        
        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true};
    }
}