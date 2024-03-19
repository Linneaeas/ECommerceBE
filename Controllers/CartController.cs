using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


    [HttpPost]
    [Authorize]
    public IActionResult AddToCart([FromQuery] int id, int quantity, Product product)
    {
        User? user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));


        CartItem cartItem = new CartItem();
        cartItem.Id = id;
        cartItem.Quantity = quantity;
        cartItem.User = user;
        cartItem.Product = product;

        user.CartItems.Add(cartItem);

        context.CartItems.Add(cartItem);
        context.SaveChanges();

        return Ok(new CartItemDto(cartItem));
    }

    [HttpGet]
    [Authorize("GetCart")]
    public List<CartItemDto> GetCartItems()
    {
        return context.CartItems.ToList().Select(cartItem => new CartItemDto(cartItem)).ToList();
    }

    [HttpPut("update/{id}")]
    [Authorize]
    public IActionResult UpdateCartItem(int id, CartItem updatedCartItem)
    {
        var existingCartItem = context.CartItems.FirstOrDefault(c => c.Id == id);
        if (existingCartItem == null)
            return NotFound("Cart item not found");

        existingCartItem.Quantity = updatedCartItem.Quantity;

        context.SaveChanges();

        return Ok("Cart item updated successfully");
    }


    [HttpDelete("remove/{id}")]
    [Authorize]
    public IActionResult RemoveFromCart(int id)
    {
        var cartItem = context.CartItems.FirstOrDefault(c => c.Id == id);
        if (cartItem == null)
            return NotFound("Cart item not found");

        context.CartItems.Remove(cartItem);
        context.SaveChanges();

        return Ok("Cart item removed successfully");
    }
}