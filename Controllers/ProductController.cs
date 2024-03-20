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
    [HttpPost("AddProduct")]
    // [Authorize]
    public IActionResult AddProduct([FromQuery] string name, string description, double price, string picture, int Inventory)
    {
        //  User? user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

        Product product = new Product();

        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.Picture = picture;
        product.Inventory = Inventory;

        context.Products.Add(product);
        context.SaveChanges();

        return Ok("Product added successfully.");
    }

    // Uppdaterar en befintlig produkt
    [HttpPut("UpdateProduct/{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        Product existingProduct = context.Products.FirstOrDefault(p => p.Id == id);

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
    [HttpDelete("DeleteProduct{id}")]
    public IActionResult DeleteProduct(int id)
    {
        Product existingProduct = context.Products.FirstOrDefault(p => p.Id == id);

        if (existingProduct == null)
            return NotFound("Product not found");

        context.Products.Remove(existingProduct);
        context.SaveChanges();

        return Ok("Product deleted successfully");
    }
}