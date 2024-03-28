
using ECommerceBE.Models;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Picture { get; set; }
    public int Inventory { get; set; }

    public ProductDto() { }

    public ProductDto(Product product)
    {
        this.Name = product.Name;
        this.Description = product.Description;
        this.Price = product.Price;
        this.Picture = product.Picture;
        this.Inventory = product.Inventory;

    }
}