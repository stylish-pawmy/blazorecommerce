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
    public async Task<ActionResult<ServiceResponse<string>>> CreateCheckoutSession()
    {
        var url = (await _paymentService.CreateCheckoutSession()).Url;
        return Ok(new ServiceResponse<string> {Data = url});
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> FullfillOrder()
    {
        var result = await _paymentService.FulfillOrder(Request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}