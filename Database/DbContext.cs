
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class MyDbContext : IdentityDbContext<User>
{
    public DbSet<Product> Products { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
    {
    }
}