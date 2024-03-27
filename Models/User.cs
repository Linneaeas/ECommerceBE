using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECommerceBE.Database;
public class User : IdentityUser
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>
    ();
}



