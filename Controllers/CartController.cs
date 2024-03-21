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

    [HttpPost("AddToCart/{productId}/{quantity}")]
    [Authorize]
    public IActionResult AddToCart(int quantity, int productId)
    {
        User? user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null)
        {
            return NotFound("User not found.");
        }
        if (quantity <= 0)
        {
            return BadRequest("Quantity should be a positive integer.");
        }
        // Retrieve the product
        Product product = context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Check if the product already exists in the user's cart
        CartItem? existingCartItem = user.CartItems.FirstOrDefault(ci => ci.Product.Id == productId);
        if (existingCartItem != null)
        {
            // If the product already exists in the cart, update its quantity
            existingCartItem.Quantity += quantity;
        }
        else
        {
            // If the product does not exist in the cart, create a new cart item
            CartItem cartItem = new CartItem
            {
                Product = product,
                Quantity = quantity
            };

            user.CartItems.Add(cartItem);
        }


        context.SaveChanges();

        return Ok("Product added to cart successfully.");
    }







    [HttpGet("GetCart")]
    [Authorize]
    public List<CartItemDto> GetCartItems()
    {
        return context.CartItems.ToList().Select(cartItem => new CartItemDto(cartItem)).ToList();
    }

    /*  [HttpPut("update/{id}")]
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
      }*/
}