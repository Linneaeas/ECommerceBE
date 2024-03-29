using System.Security.Claims;
using ECommerceBE.Database;
using ECommerceBE.Models;
using Microsoft.EntityFrameworkCore;

public class CartService
{
    private readonly MyDbContext context;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CartService(MyDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?
        .User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public void AddToCart(int productId, int quantity)
    {
        string userId = GetCurrentUserId();

        if (userId == null)
        {
            throw new ApplicationException("User not authenticated.");
        }

        User user = context.Users
            .Include(u => u.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new ApplicationException("User not found.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity should be a positive integer.");
        }

        Product product = context.Products
        .FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            throw new ApplicationException("Product not found.");
        }

        CartItem existingCartItem = user.CartItems
        .FirstOrDefault(ci => ci.Product.Id == productId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }

        else
        {
            CartItem cartItem = new CartItem
            {
                Product = product,
                Quantity = quantity,
                User = user
            };
            user.CartItems.Add(cartItem);
        }
        context.SaveChanges();
    }

    public List<CartItemDto> GetCartItems()
    {
        string userId = GetCurrentUserId();

        if (userId == null)
        {
            throw new ApplicationException("User not authenticated.");
        }

        var cartItems = context.CartItems
          .Include(ci => ci.Product)
          .Where(ci => ci.User.Id == userId)
          .Select(ci => new CartItemDto

          {
              Quantity = ci.Quantity,
              ProductName = ci.Product.Name
          })

          .ToList();

        return cartItems;
    }

    public void RemoveFromCart(int cartItemId)
    {
        string userId = GetCurrentUserId();

        if (userId == null)
        {
            throw new ApplicationException("User not authenticated.");
        }

        CartItem existingCartItem = context.CartItems
           .Include(ci => ci.User)
           .FirstOrDefault(ci => ci.Id == cartItemId && ci.User.Id == userId);

        if (existingCartItem == null)
        {
            throw new ApplicationException("Item not found or does not belong to the current user.");
        }

        context.CartItems.Remove(existingCartItem);
        context.SaveChanges();


    }
}
