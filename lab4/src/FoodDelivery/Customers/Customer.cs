namespace FoodDelivery.Domain.Customers;

public class Customer
{
    public string Id { get; }
    public string Name { get; }

    public Customer(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
