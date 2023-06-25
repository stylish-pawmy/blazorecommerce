namespace BlazorEcommerce.Shared;

public class OrderOverviewResponse
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public string Product { get; set; } = null!;
    public string ProductImageUrl { get; set; } = string.Empty;
}