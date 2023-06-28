using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
    {
        var response = await _addressService.GetAddress();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
    {
        var response = await _addressService.AddOrUpdateAddress(address);
        return Ok(address);
    }
}