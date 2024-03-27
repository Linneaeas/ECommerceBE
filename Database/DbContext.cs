
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceBE.Models;


namespace ECommerceBE.Database
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<CompletedOrderItem> CompletedOrderItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options)
                : base(options)
        {
        }
    }
}