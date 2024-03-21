using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class CartItem
{

    [Key]
    public int Id { get; set; }

    public int Quantity { get; set; }

    public User User { get; set; }
    public Product Product { get; set; }
}
public class CartItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public CartItemDto(CartItem cartItem)
    {
        this.Id = cartItem.Id;
        this.Quantity = cartItem.Quantity;
    }

    public CartItemDto()
    {
    }
}

