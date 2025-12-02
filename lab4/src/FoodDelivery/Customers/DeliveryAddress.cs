namespace FoodDelivery.Domain.Customers;

public class DeliveryAddress
{
    public string City { get; }
    public string Street { get; }
    public string House { get; }

    public DeliveryAddress(string city, string street, string house)
    {
        City = city;
        Street = street;
        House = house;
    }

    public override string ToString() => $"{City}, {Street}, {House}";
}
