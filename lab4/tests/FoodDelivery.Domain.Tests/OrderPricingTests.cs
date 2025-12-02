using FoodDelivery.Domain.Customers;
using FoodDelivery.Domain.Menu;
using FoodDelivery.Domain.Orders;
using FoodDelivery.Domain.Pricing;

namespace FoodDelivery.Domain.Tests;

public class OrderPricingTests
{
    private readonly Customer _customer = new("1", "Test");
    private readonly DeliveryAddress _address = new("City", "Street", "1");
    private readonly MenuItem _pizza = new("1", "Pizza", 500);

    [Fact]
    public void StandardPricing_CalculatesItemsOnly()
    {
        var builder = new OrderBuilder()
            .ForCustomer(_customer)
            .ToAddress(_address)
            .WithPricingStrategy(new StandardPricingStrategy())
            .AddItem(_pizza, 2);

        var order = builder.Build();

        Assert.Equal(1000m, order.CalculateTotal());
    }

    [Fact]
    public void FastDelivery_AddsDeliveryFee()
    {
        var builder = new OrderBuilder()
            .ForCustomer(_customer)
            .ToAddress(_address)
            .WithPricingStrategy(new FastDeliveryPricingStrategy(200))
            .AddItem(_pizza, 1);

        var order = builder.Build();

        Assert.Equal(700m, order.CalculateTotal());
    }

    [Fact]
    public void DiscountPricing_AppliesPercentDiscount()
    {
        var builder = new OrderBuilder()
            .ForCustomer(_customer)
            .ToAddress(_address)
            .WithPricingStrategy(new DiscountPricingStrategy(10))
            .AddItem(_pizza, 2);

        var order = builder.Build();

        Assert.Equal(900m, order.CalculateTotal());
    }
}
