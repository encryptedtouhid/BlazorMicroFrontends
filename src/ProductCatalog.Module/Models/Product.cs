namespace ProductCatalog.Module.Models;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; } = 0;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public List<string> Tags { get; set; } = new();
    public decimal? DiscountPercent { get; set; }
    public decimal DiscountedPrice => DiscountPercent.HasValue ? Price * (1 - DiscountPercent.Value / 100) : Price;
}

public class ProductCategory
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProductCount { get; set; } = 0;
}
