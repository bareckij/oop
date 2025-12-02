using FoodDelivery.Domain.Customers;
using FoodDelivery.Domain.Menu;
using FoodDelivery.Domain.Orders;
using FoodDelivery.Domain.Pricing;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var menu = new List<MenuItem>
{
    new("1", "Пицца Маргарита", 500),
    new("2", "Бургер", 350),
    new("3", "Салат Цезарь", 300)
};

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Служба доставки еды ===");
    Console.WriteLine("1. Создать стандартный заказ");
    Console.WriteLine("2. Создать заказ с быстрой доставкой");
    Console.WriteLine("3. Создать заказ со скидкой");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите пункт: ");
    var choice = Console.ReadLine();

    if (choice == "0") break;

    IPricingStrategy strategy = choice switch
    {
        "2" => new FastDeliveryPricingStrategy(200),
        "3" => new DiscountPricingStrategy(10),
        _ => new StandardPricingStrategy()
    };

    var customer = new Customer(Guid.NewGuid().ToString(), "Иван");
    var address = new DeliveryAddress("Москва", "Тверская", "1");

    var builder = new OrderBuilder()
        .ForCustomer(customer)
        .ToAddress(address)
        .WithPricingStrategy(strategy);

    Console.WriteLine("\nМеню:");
    foreach (var item in menu)
    {
        Console.WriteLine($"{item.Id}. {item.Name} - {item.BasePrice}₽");
    }

    while (true)
    {
        Console.Write("Введите id блюда (или пусто для завершения): ");
        var id = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id)) break;
        var menuItem = menu.FirstOrDefault(m => m.Id == id);
        if (menuItem is null)
        {
            Console.WriteLine("Блюдо не найдено.");
            continue;
        }

        Console.Write("Количество: ");
        int.TryParse(Console.ReadLine(), out var qty);
        if (qty <= 0) qty = 1;

        builder.AddItem(menuItem, qty);
    }

    var order = builder.Build();

    Console.WriteLine($"\nЗаказ создан. Клиент: {order.Customer.Name}, адрес: {order.Address}");
    Console.WriteLine($"Статус: {order.Status}");
    Console.WriteLine($"Сумма: {order.CalculateTotal()}₽");

    order.MoveToPreparing();
    Console.WriteLine($"Статус после подготовки: {order.Status}");
    order.MoveToDelivering();
    Console.WriteLine($"Статус после передачи курьеру: {order.Status}");
    order.MoveToCompleted();
    Console.WriteLine($"Статус после завершения: {order.Status}");

    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
}
