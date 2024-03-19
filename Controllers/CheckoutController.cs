using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;

[ApiController]
[Route("[controller]")]
public class CheckOutController : ControllerBase
{
    MyDbContext context;
    UserManager<User> userManager;
    RoleManager<IdentityRole> roleManager;

    public CheckOutController(MyDbContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }


    [HttpDelete]
    [Authorize]
    public IActionResult CompleteCheckout(User user)
    {
        try
        {
            var cartItems = context.CartItems.Where(c => c.User.Equals(user)).ToList();

            context.CartItems.RemoveRange(cartItems);
            context.SaveChanges();

            return Ok("Checkout completed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}