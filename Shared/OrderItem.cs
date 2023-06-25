namespace BlazorEcommerce.Shared;

public class OrderItem
{
    public Order Order { get; set; } = null!;
    public int OrderId { get; set; }
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public ProductType ProductType { get; set; } = null!;
    public int ProductTypeId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
}