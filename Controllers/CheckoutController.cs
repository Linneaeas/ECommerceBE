using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceBE.Database;

[ApiController]
[Route("[controller]")]
public class CheckOutController : ControllerBase
{
    MyDbContext context;
    UserManager<User> userManager;

    public CheckOutController(MyDbContext context, UserManager<User> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    [HttpPost("CompleteCheckout")]
    [Authorize]
    public IActionResult CompleteCheckout()
    {
        try
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.Users.Find(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cartItems = context.CartItems
                .Where(c => c.User.Id == userId)
                .Include(c => c.Product)
                .ToList();

            if (!cartItems.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            var completedOrder = new CompletedOrder
            {
                UserId = userId,
                Items = cartItems.Select(c => new CompletedOrderItem

                {
                    ProductId = c.Product.Id,
                    Quantity = c.Quantity
                })
                .ToList()
            };

            context.CompletedOrders.Add(completedOrder);
            context.CartItems.RemoveRange(cartItems);
            context.SaveChanges();

            return Ok("Checkout completed successfully.");
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: Something went wrong, try again or contact customer service for assistance");
        }
    }
}


