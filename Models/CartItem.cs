using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ECommerceBE.Models;
using ECommerceBE.Database;

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


