namespace BlazorEcommerce.Server.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;

    public AddressService(IAuthService authService, ApplicationDbContext context)
    {
        this._authService = authService;
        this._context = context;
    }
    public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
    {
        var userId = _authService.GetUserId();
        var response = new ServiceResponse<Address>();
        var dbAddress = (await GetAddress()).Data;

        if (dbAddress is null)
        {
            address.UserId = userId;
            _context.Addresses.Add(address);
        }
        else {
            dbAddress.City = address.City;
            dbAddress.Country = address.Country;
            dbAddress.FirstName = address.FirstName;
            dbAddress.LastName = address.LastName;
            dbAddress.State = dbAddress.State;
            dbAddress.Street = dbAddress.Street;
            dbAddress.Zip = dbAddress.Zip;
            response.Data = dbAddress;
        }

        response.Data = address;
        await _context.SaveChangesAsync();
        return response;
    }

    public async Task<ServiceResponse<Address>> GetAddress()
    {
        var userId = _authService.GetUserId();
        var address = await _context.Addresses.Where(a => a.UserId == userId)
            .FirstOrDefaultAsync();
        
        return new ServiceResponse<Address>() {Data = address};
    }
}