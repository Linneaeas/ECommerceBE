using ECommerceBE.Models;
using ECommerceBE.Database;
public class CompletedOrder
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public List<CompletedOrderItem> Items { get; set; } = new List<CompletedOrderItem>();
}

public class CompletedOrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public int CompletedOrderId { get; set; }
    public CompletedOrder CompletedOrder { get; set; }
}
