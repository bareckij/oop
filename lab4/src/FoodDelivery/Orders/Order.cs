using FoodDelivery.Domain.Customers;
using FoodDelivery.Domain.Pricing;

namespace FoodDelivery.Domain.Orders;

public class Order
{
    private readonly List<OrderItem> _items = new();
    private OrderState _state = new CreatedState();

    public string Id { get; }
    public Customer Customer { get; }
    public DeliveryAddress Address { get; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status => _state.Status;
    public IPricingStrategy PricingStrategy { get; private set; }

    public Order(string id, Customer customer, DeliveryAddress address, IPricingStrategy pricingStrategy)
    {
        Id = id;
        Customer = customer;
        Address = address;
        PricingStrategy = pricingStrategy;
    }

    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Нельзя изменять состав заказа после начала подготовки.");
        _items.Add(item);
    }

    public void SetPricingStrategy(IPricingStrategy strategy)
    {
        PricingStrategy = strategy;
    }

    public decimal CalculateTotal() => PricingStrategy.CalculateTotal(this);

    public void MoveToPreparing() => _state = _state.MoveToPreparing();
    public void MoveToDelivering() => _state = _state.MoveToDelivering();
    public void MoveToCompleted() => _state = _state.MoveToCompleted();
}
