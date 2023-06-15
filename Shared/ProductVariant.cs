namespace BlazorEcommerce.Shared;

using System.Text.Json.Serialization;

public class ProductVariant
{
    [JsonIgnore]
    public Product? Product { get; set; }
    public int ProductId { get; set; }
    public ProductType? ProductType { get; set; }
    public int ProductTypeId { get; set; }

    public double Price { get; set; }
    public double OriginalPrice { get; set; }
}