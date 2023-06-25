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

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
    {
        var response = await _cartService.AddToCart(cartItem);
        return Ok(response);
    }

    [HttpPut("update-quantity")]
    public async Task<ActionResult<ServiceResponse<bool>>> UpdateProductQuantity(CartItem cartItem)
    {
        var response = await _cartService.UpdateProductQuantity(cartItem);
        return Ok(response);
    }

    [HttpDelete("{productId}/{productTypeId}")]
    public async Task<ActionResult<ServiceResponse<bool>>> RemoveProduct(int productId, int productTypeId)
    {
        var response = await _cartService.RemoveProduct(productId, productTypeId);
        return Ok(response);
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartProducts(List<CartItem> cartItems)
    {
        var response = await _cartService.StoreCartProducts(cartItems);
        
        return Ok(response);
    }

    [HttpGet("count")]
    public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
    {
        return Ok(await _cartService.GetCartItemsCount());
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<CartProductResponse>>> GetDbCartItemsCount()
    {
        return Ok(await _cartService.GetDbCartProducts());
    }
}