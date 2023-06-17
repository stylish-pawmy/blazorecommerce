namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

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
                ProductTypeId = variant.ProductTypeId
            };

            response.Data.Add(productResponse);
        }

        return response;
    }
}