using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>
    ();

}



