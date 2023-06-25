namespace BlazorEcommerce.Shared;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
}