using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICartService _cartService;

    public OrderService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ICartService cartService
    )
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _cartService = cartService;
    }

    private int GetUserId() => int.Parse(
        _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<ServiceResponse<bool>> PlaceOrder()
    {
        var products = (await _cartService.GetDbCartProducts()).Data;
        double totalPrice = 0;
        products.ForEach(product => totalPrice += product.Price * product.Quantity);

        var orderItems = new List<OrderItem>();
        products.ForEach(product => orderItems.Add(
            new OrderItem {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }
        ));

        var order = new Order {
            UserId = GetUserId(),
            OrderDate = DateTime.Now.ToUniversalTime(),
            OrderItems = orderItems,
            TotalPrice = totalPrice
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new ServiceResponse<bool> {Data = true};
    }
}