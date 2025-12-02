using FoodDelivery.Domain.Menu;

namespace FoodDelivery.Domain.Orders;

public class OrderItem
{
    public MenuItem Item { get; }
    public int Quantity { get; }

    public OrderItem(MenuItem item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Item = item;
        Quantity = quantity;
    }
}
