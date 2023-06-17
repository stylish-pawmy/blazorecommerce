namespace BlazorEcommerce.Shared;

public class CartProductResponse
{
    public int ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public double Price { get; set; }
    public int ProductTypeId { get; set; }
    public string ProductType { get; set; } = string.Empty;
}