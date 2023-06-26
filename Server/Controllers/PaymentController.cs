using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("checkout")]
    public async Task<ServiceResponse<string>> CreateCheckoutSession()
    {
        var url = (await _paymentService.CreateCheckoutSession()).Url;
        return new ServiceResponse<string> {Data = url};
    }
}