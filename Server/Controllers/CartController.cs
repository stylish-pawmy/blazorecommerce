using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("products")]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
    {
        var response = await _cartService.GetCartProducts(cartItems);
        return Ok(response);
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartProducts(List<CartItem> cartItems)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _cartService.StoreCartProducts(cartItems, userId);
        
        return Ok(response);
    }
}