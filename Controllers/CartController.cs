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

    //Vi kanske kan ha AddToCart som alltid lägger till 1 av den produkten/skapar en ny om den inte finns, istället för att skriva in quantity så är quantity alltid 1 och ökas med 1 varje gång man gör request? samma sak med remove from cart fast tvärtom? Då baserat på ProductId (of that user) 
    //ELLER, AddNewProductToCart som lägger till en product plus quantity, när man vill ändra quantity har vi en annan som okar quantity pa en product som redan finns i cart
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

            user.CartItems.Add(cartItem); //Maste sta user annars sparas det inte kopplat till anvandaren
        }


        context.SaveChanges();

        return Ok("Product added to cart successfully.");
    }


    [HttpGet("GetCart")]
    [Authorize]
    public List<CartItem> GetAllCartItems()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return GetAllCartItems(userId).Select(CartItem => new CartItem()).ToList();
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
*/

    [HttpDelete("RemoveFromCart/{cartItemId}")]
    //[Authorize]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        CartItem existingCartItem = context.CartItems.FirstOrDefault(c => c.Id == cartItemId);

        if (existingCartItem == null)
            return NotFound("Item not found");

        context.CartItems.Remove(existingCartItem);
        context.SaveChanges();

        return Ok("Item deleted successfully");
    }


    public List<CartItem> GetAllCartItems(string userId)
    {
        User? user = context.Users.Find(userId);
        if (user == null)
        {
            return new List<CartItem>();
        }

        return context.CartItems.Where(CartItem => CartItem.User.Id == user.Id).ToList();
    }

}




