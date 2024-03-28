using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceBE.Models;
using ECommerceBE.Database;


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
    [HttpPost("AddProduct")]
    // [Authorize]
    public IActionResult AddProduct(ProductDto productDto)
    {
        //   User? user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

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

        return Ok(new ProductDto(product));
    }



    [HttpPut("UpdateProduct/{productId}")]
    public IActionResult UpdateProduct(int productId, Product product)
    {

        Product existingProduct = context.Products.FirstOrDefault(p => p.Id == productId);

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

    [HttpDelete("DeleteProduct/{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        Product existingProduct = context.Products.FirstOrDefault(p => p.Id == productId);

        if (existingProduct == null)
            return NotFound("Product not found");

        context.Products.Remove(existingProduct);
        context.SaveChanges();

        return Ok("Product deleted successfully");
    }
}