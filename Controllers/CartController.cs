using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceBE.Database;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly CartService cartService;

    public CartController(CartService cartService)
    {
        this.cartService = cartService;
    }

    [HttpPost("AddToCart/{productId}/{quantity}")]
    [Authorize]
    public IActionResult AddToCart(int productId, int quantity)
    {
        try
        {
            cartService.AddToCart(productId, quantity);
            return Ok("Product added to cart successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetCartItems")]
    [Authorize]
    public IActionResult GetCartItems()
    {
        try
        {
            var cartItems = cartService.GetCartItems();
            return Ok(cartItems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("RemoveFromCart/{cartItemId}")]
    [Authorize]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        try
        {
            cartService.RemoveFromCart(cartItemId);
            return Ok("Cart item removed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

