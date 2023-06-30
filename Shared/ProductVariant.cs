using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared;

public class ProductVariant
{
    [JsonIgnore]
    public Product? Product { get; set; }
    public int ProductId { get; set; }
    public ProductType? ProductType { get; set; }
    public int ProductTypeId { get; set; }

    public double Price { get; set; }
    public double OriginalPrice { get; set; }

    public bool Deleted { get; set; } = false;
    public bool Visible { get; set; } = true;

    [NotMapped]
    public bool Editing { get; set; } = false;
    [NotMapped]
    public bool IsNew { get; set; } = false;
}