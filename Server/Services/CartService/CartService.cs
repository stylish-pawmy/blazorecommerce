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
        await _context.CartItems.AddRangeAsync(cartItems);
        await _context.SaveChangesAsync();

        return await GetCartProducts(
            await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()
        );
    }
}