using FoodDelivery.Domain.Customers;
using FoodDelivery.Domain.Menu;
using FoodDelivery.Domain.Pricing;

namespace FoodDelivery.Domain.Orders;

public class OrderBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private Customer? _customer;
    private DeliveryAddress? _address;
    private readonly List<OrderItem> _items = new();
    private IPricingStrategy _pricingStrategy = new StandardPricingStrategy();

    public OrderBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public OrderBuilder ForCustomer(Customer customer)
    {
        _customer = customer;
        return this;
    }

    public OrderBuilder ToAddress(DeliveryAddress address)
    {
        _address = address;
        return this;
    }

    public OrderBuilder AddItem(MenuItem item, int quantity)
    {
        _items.Add(new OrderItem(item, quantity));
        return this;
    }

    public OrderBuilder WithPricingStrategy(IPricingStrategy strategy)
    {
        _pricingStrategy = strategy;
        return this;
    }

    public Order Build()
    {
        if (_customer is null) throw new InvalidOperationException("Не указан клиент.");
        if (_address is null) throw new InvalidOperationException("Не указан адрес доставки.");
        var order = new Order(_id, _customer, _address, _pricingStrategy);
        foreach (var item in _items)
        {
            order.AddItem(item);
        }

        return order;
    }
}
