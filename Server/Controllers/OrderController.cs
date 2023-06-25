using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
    {
        var response = await _orderService.PlaceOrder();
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<OrderOverviewResponse>>> GetOrders()
    {
        var response = await _orderService.GetOrders();
        return Ok(response);
    }
}