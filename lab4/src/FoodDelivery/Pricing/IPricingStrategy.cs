using FoodDelivery.Domain.Orders;

namespace FoodDelivery.Domain.Pricing;

public interface IPricingStrategy
{
    decimal CalculateTotal(Order order);
}

public class StandardPricingStrategy : IPricingStrategy
{
    public decimal CalculateTotal(Order order)
    {
        var itemsTotal = order.Items.Sum(i => i.Item.BasePrice * i.Quantity);
        return itemsTotal;
    }
}

public class FastDeliveryPricingStrategy : IPricingStrategy
{
    private readonly decimal _deliveryFee;

    public FastDeliveryPricingStrategy(decimal deliveryFee)
    {
        _deliveryFee = deliveryFee;
    }

    public decimal CalculateTotal(Order order)
    {
        var itemsTotal = order.Items.Sum(i => i.Item.BasePrice * i.Quantity);
        return itemsTotal + _deliveryFee;
    }
}

public class DiscountPricingStrategy : IPricingStrategy
{
    private readonly decimal _discountPercent;

    public DiscountPricingStrategy(decimal discountPercent)
    {
        _discountPercent = discountPercent;
    }

    public decimal CalculateTotal(Order order)
    {
        var itemsTotal = order.Items.Sum(i => i.Item.BasePrice * i.Quantity);
        var discount = itemsTotal * _discountPercent / 100m;
        return itemsTotal - discount;
    }
}
