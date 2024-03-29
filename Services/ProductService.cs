using ECommerceBE.Models;
using ECommerceBE.Database;

public class ProductService
{
    private readonly MyDbContext context;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ProductService(MyDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.httpContextAccessor = httpContextAccessor;
    }

    public void AddProduct(ProductDto productDto)
    {

        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Picture = productDto.Picture,
            Inventory = productDto.Inventory
        };

        context.Products.Add(product);
        context.SaveChanges();
    }

    public void UpdateProduct(int productId, Product product)
    {
        Product existingProduct = context.Products
        .FirstOrDefault(p => p.Id == productId);

        if (existingProduct == null)
        {
            throw new ApplicationException("Product not found.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Picture = product.Picture;
        existingProduct.Inventory = product.Inventory;

        context.SaveChanges();
    }

    public void DeleteProduct(int productId)
    {
        Product existingProduct = context.Products
        .FirstOrDefault(p => p.Id == productId);

        if (existingProduct == null)
        {
            throw new ApplicationException("Product not found");
        }

        context.Products.Remove(existingProduct);
        context.SaveChanges();
    }
}