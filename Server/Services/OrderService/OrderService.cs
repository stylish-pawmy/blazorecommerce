using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;

    public OrderService(
        ApplicationDbContext context,
        IAuthService authService,
        ICartService cartService
    )
    {
        _context = context;
        _authService = authService;
        _cartService = cartService;
    }

    public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
    {
        var response = new ServiceResponse<List<OrderOverviewResponse>>();

        var orders = await _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .Where(o => o.UserId == _authService.GetUserId())
        .OrderBy(o => o.OrderDate)
        .ToListAsync();

        var orderResponse = new List<OrderOverviewResponse>();

        orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            OrderDate = o.OrderDate,
            Product =  (o.OrderItems.Count > 1) ?
                        $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more."
                        : $"{o.OrderItems.First().Product.Title}.",
            ProductImageUrl = o.OrderItems.First().Product.ImageUrl
        }));

        response.Data = orderResponse;

        return response;
    }

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
            UserId = _authService.GetUserId(),
            OrderDate = DateTime.Now.ToUniversalTime(),
            OrderItems = orderItems,
            TotalPrice = totalPrice
        };

        _context.Orders.Add(order);

        _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()));

        await _context.SaveChangesAsync();

        return new ServiceResponse<bool> {Data = true};
    }
}