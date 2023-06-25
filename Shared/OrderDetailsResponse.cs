namespace BlazorEcommerce.Shared;

public class OrderDetailsResponse
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public List<OrderProductDetailsResponse> Products { get; set; } = null!;
}