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
public class CartController : ControllerBase
{
    MyDbContext context;
    UserManager<User> userManager;
    RoleManager<IdentityRole> roleManager;
    public CartController(
        MyDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    [HttpPost("AddToCart/{productId}/{quantity}")]
    [Authorize]
    public IActionResult AddToCart(int quantity, int productId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        User user = context.Users.Include(u => u.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        if (quantity <= 0)
        {
            return BadRequest("Quantity should be a positive integer.");
        }
        Product product = context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }
        CartItem existingCartItem = user.CartItems.FirstOrDefault(ci => ci.Product.Id == productId);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            CartItem cartItem = new CartItem
            {
                Product = product,
                Quantity = quantity,
                User = user
            };

            user.CartItems.Add(cartItem);
        }

        context.SaveChanges();
        return Ok("Product added to cart successfully.");
    }

    [HttpGet("GetCartItems")]
    [Authorize]
    public List<CartItemDto> GetCartItems()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItems = context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.User.Id == userId)
            .Select(ci => new CartItemDto
            {
                Quantity = ci.Quantity,
                ProductName = ci.Product.Name
            })
            .ToList();
        return cartItems;
    }
    [HttpDelete("RemoveFromCart/{cartItemId}")]
    [Authorize]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        CartItem existingCartItem = context.CartItems
            .Include(ci => ci.User)
            .FirstOrDefault(ci => ci.Id == cartItemId && ci.User.Id == userId);


        if (existingCartItem == null)
        {
            return NotFound("Item not found or does not belong to the current user");
        }

        context.CartItems.Remove(existingCartItem);
        context.SaveChanges();

        return Ok("Item deleted successfully");
    }

}

