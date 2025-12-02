using FoodDelivery.Domain.Customers;
using FoodDelivery.Domain.Menu;
using FoodDelivery.Domain.Orders;
using FoodDelivery.Domain.Pricing;

namespace FoodDelivery.Domain.Tests;

public class OrderStateTests
{
    private Order CreateOrder()
    {
        var customer = new Customer("1", "Test");
        var address = new DeliveryAddress("City", "Street", "1");
        var builder = new OrderBuilder()
            .ForCustomer(customer)
            .ToAddress(address)
            .WithPricingStrategy(new StandardPricingStrategy())
            .AddItem(new MenuItem("1", "Pizza", 500), 1);

        return builder.Build();
    }

    [Fact]
    public void Order_MovesThroughStatesCorrectly()
    {
        var order = CreateOrder();

        Assert.Equal(OrderStatus.Created, order.Status);
        order.MoveToPreparing();
        Assert.Equal(OrderStatus.Preparing, order.Status);
        order.MoveToDelivering();
        Assert.Equal(OrderStatus.Delivering, order.Status);
        order.MoveToCompleted();
        Assert.Equal(OrderStatus.Completed, order.Status);
    }

    [Fact]
    public void CannotChangeItemsAfterPreparing()
    {
        var order = CreateOrder();
        order.MoveToPreparing();

        Assert.Throws<InvalidOperationException>(() =>
            order.AddItem(new OrderItem(new MenuItem("2", "Burger", 300), 1)));
    }
}
