namespace BlazorEcommerce.Client.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly HttpClient _http;

    public AddressService(HttpClient http)
    {
        this._http = http;
    }
    public async Task<Address> AddOrUpdateAddress(Address address)
    {
        var response = await _http.PostAsJsonAsync<Address>("api/address", address);
        return (await response.Content.ReadFromJsonAsync<ServiceResponse<Address>>()).Data;
    }

    public async Task<Address> GetAddress()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<Address>>("/api/address");
        return response.Data;
    }
}