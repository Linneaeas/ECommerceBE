using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    MyDbContext context;
    UserManager<User> userManager;
    RoleManager<IdentityRole> roleManager;

    public ProductController(
        MyDbContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }


    // Lägger till en ny produkt
    [HttpPost]
    [Authorize]
    public IActionResult AddProduct([FromQuery] string name, int id, string description, double price, string picture, int Inventory)
    {
        User? user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

        Product product = new Product();
        product.Id = id;
        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.Picture = picture;
        product.Inventory = Inventory;

        context.Products.Add(product);
        context.SaveChanges();

        return Ok(new ProductDto(product));
    }

    // Uppdaterar en befintlig produkt
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        var existingProduct = context.Products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
            return NotFound("Product not found");

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Picture = product.Picture;
        existingProduct.Inventory = product.Inventory;

        context.SaveChanges();

        return Ok("Product updated successfully");
    }

    // Tar bort en produkt
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound("Product not found");

        context.Products.Remove(product);
        context.SaveChanges();

        return Ok("Product deleted successfully");
    }
}