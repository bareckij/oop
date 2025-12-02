namespace FoodDelivery.Domain.Menu;

public class MenuItem
{
    public string Id { get; }
    public string Name { get; }
    public decimal BasePrice { get; }

    public MenuItem(string id, string name, decimal basePrice)
    {
        Id = id;
        Name = name;
        BasePrice = basePrice;
    }
}
