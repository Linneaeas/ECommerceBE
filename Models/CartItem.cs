using System.ComponentModel.DataAnnotations;
using ECommerceBE.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int Quantity { get; set; }

    public User User { get; set; }
    public Product Product { get; set; }

    public CartItem()
    {
    }
}


