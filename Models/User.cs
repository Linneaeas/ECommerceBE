
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class User : IdentityUser
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>
    ();
}



