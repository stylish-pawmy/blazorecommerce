using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IConfiguration _config;

    public PaymentService(
        IAuthService authService,
        ICartService cartService,
        IOrderService orderService,
        IConfiguration config
    )
    {
        _authService = authService;
        _orderService = orderService;
        _cartService = cartService;
        _config = config;
        StripeConfiguration.ApiKey = _config.GetSection("Stripe:Secret").Value;
    }

    public async Task<Session> CreateCheckoutSession()
    {
        var products = (await _cartService.GetDbCartProducts()).Data;
        List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();

        products.ForEach(p => lineItems.Add(new SessionLineItemOptions{
            PriceData = new SessionLineItemPriceDataOptions {
                UnitAmountDecimal = Convert.ToDecimal(p.Price),
                Currency = "dzd",
                ProductData = new SessionLineItemPriceDataProductDataOptions {
                    Name = p.Title,
                    Images = new List<string> {
                        p.ImageUrl
                    }
                }
            },
            Quantity = p.Quantity
        }));

        var sessionOptions = new SessionCreateOptions {
            CustomerEmail = _authService.GetUserEmail(),
            PaymentMethodTypes = new List<string> {
                "card"
            },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://localhost:7146/payment-success",
            CancelUrl = "https://localhost:7146/cart"
        };

        var sessionService = new SessionService();
        var session = sessionService.Create(sessionOptions);

        return session;
    }
}