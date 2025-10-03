using System;
using System.Collections.Generic;

namespace lab1
{
    public class AdminPanel
    {
        private readonly VendingMachine _vendingMachine;

        public AdminPanel(VendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine ?? throw new ArgumentNullException(nameof(vendingMachine));
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПАНЕЛЬ АДМИНИСТРАТОРА ===");
                Console.WriteLine("1. Показать все товары");
                Console.WriteLine("2. Добавить новый товар");
                Console.WriteLine("3. Пополнить товар");
                Console.WriteLine("4. Собрать деньги");
                Console.WriteLine("5. Пополнить автомат монетами");
                Console.WriteLine("6. Показать состояние автомата");
                Console.WriteLine("7. Выход из админ-панели");
                Console.WriteLine("===============================");
                Console.Write("Выберите действие (1-7): ");

                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                switch (input.Trim())
                {
                    case "1":
                        ShowAllProducts();
                        break;
                    case "2":
                        AddNewProduct();
                        break;
                    case "3":
                        RestockProduct();
                        break;
                    case "4":
                        CollectMoney();
                        break;
                    case "5":
                        AddMoneyToMachine();
                        break;
                    case "6":
                        ShowMachineStatus();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowAllProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ ТОВАРЫ ===");
            
            var products = _vendingMachine.GetProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("Товары отсутствуют.");
            }
            else
            {
                Console.WriteLine($"{"Код",-4} {"Название",-20} {"Цена (руб.)",-12} {"Количество",-10} {"Статус",-15}");
                Console.WriteLine(new string('-', 65));
                
                foreach (var product in products)
                {
                    string status = product.IsAvailable ? "В наличии" : "Нет в наличии";
                    Console.WriteLine($"{product.Code,-4} {product.Name,-20} {product.Price,-12:F0} {product.Quantity,-10} {status,-15}");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        private void AddNewProduct()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО ТОВАРА ===");
                Console.WriteLine("0. Вернуться в админ-меню");

                try
                {
                    string? code;
                    while (true)
                    {
                        Console.Write("\nВведите код товара (например, A3): ");
                        code = Console.ReadLine()?.Trim().ToUpper();
                        if (code == "0")
                            return;
                        
                        if (string.IsNullOrWhiteSpace(code))
                        {
                            Console.WriteLine("Код товара не может быть пустым. Попробуйте снова.");
                            continue;
                        }
                        
                        var existingProduct = _vendingMachine.GetProduct(code);
                        if (existingProduct != null)
                        {
                            Console.WriteLine($"Товар с кодом {code} уже существует. Попробуйте другой код.");
                            continue;
                        }
                        
                        break;
                    }

                    string? name;
                    while (true)
                    {
                        Console.Write("Введите название товара (или 0 для отмены): ");
                        name = Console.ReadLine()?.Trim();
                        if (name == "0")
                            return;
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            Console.WriteLine("Название товара не может быть пустым. Попробуйте снова.");
                            continue;
                        }
                        break;
                    }

                    decimal price;
                    while (true)
                    {
                        Console.Write("Введите цену (руб.) или 0 для отмены: ");
                        string? priceInput = Console.ReadLine()?.Trim();
                        if (priceInput == "0")
                            return;
                        if (!decimal.TryParse(priceInput, out price) || price < 0)
                        {
                            Console.WriteLine("Неверная цена. Цена должна быть положительным числом. Попробуйте снова.");
                            continue;
                        }
                        break;
                    }

                    int quantity;
                    while (true)
                    {
                        Console.Write("Введите количество (или 0 для отмены): ");
                        string? quantityInput = Console.ReadLine()?.Trim();
                        if (quantityInput == "0")
                            return;
                        if (!int.TryParse(quantityInput, out quantity) || quantity < 0)
                        {
                            Console.WriteLine("Неверное количество. Количество должно быть неотрицательным числом. Попробуйте снова.");
                            continue;
                        }
                        break;
                    }

                    var newProduct = new Product(name, price, quantity, code);
                    _vendingMachine.AddProduct(newProduct);

                    Console.WriteLine($"\nТовар '{name}' (код: {code}) успешно добавлен!");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
                    Console.WriteLine("Нажмите любую клавишу для повтора...");
                    Console.ReadKey();
                }
            }
        }

        private void RestockProduct()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПОПОЛНЕНИЕ ТОВАРА ===");
                Console.WriteLine("0. Вернуться в админ-меню");

                string? code;
                while (true)
                {
                    Console.Write("\nВведите код товара для пополнения: ");
                    code = Console.ReadLine()?.Trim().ToUpper();
                    if (code == "0")
                        return;
                    
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        Console.WriteLine("Код товара не может быть пустым. Попробуйте снова.");
                        continue;
                    }

                    var product = _vendingMachine.GetProduct(code);
                    if (product == null)
                    {
                        Console.WriteLine($"Товар с кодом {code} не найден. Попробуйте другой код.");
                        continue;
                    }

                    Console.WriteLine($"Текущий товар: {product}");
                    break;
                }

                var selectedProduct = _vendingMachine.GetProduct(code);
                
                while (true)
                {
                    Console.Write("Введите количество для добавления (или 0 для отмены): ");
                    string? amountInput = Console.ReadLine()?.Trim();
                    
                    if (amountInput == "0")
                        break;
                    
                    if (!int.TryParse(amountInput, out int amount) || amount <= 0)
                    {
                        Console.WriteLine("Неверное количество. Количество должно быть положительным числом. Попробуйте снова.");
                        continue;
                    }

                    if (_vendingMachine.RestockProduct(code, amount) && selectedProduct != null)
                    {
                        Console.WriteLine($"\nТовар '{selectedProduct.Name}' успешно пополнен на {amount} шт.");
                        Console.WriteLine($"Новое количество: {selectedProduct.Quantity} шт.");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при пополнении товара. Попробуйте снова.");
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }

        private void CollectMoney()
        {
            Console.Clear();
            Console.WriteLine("=== СБОР ДЕНЕГ ===");

            Console.WriteLine("Текущее состояние автомата:");
            Console.WriteLine(_vendingMachine.GetMachineMoneyInfo());

            Console.Write("\nВы уверены, что хотите собрать все деньги? (да/нет): ");
            string? confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "да" || confirmation == "yes" || confirmation == "y")
            {
                decimal collectedAmount = _vendingMachine.CollectMoney();
                Console.WriteLine($"\nСобрано денег: {collectedAmount:F0} руб.");
                
                if (collectedAmount > 0)
                {
                    Console.WriteLine("Все деньги успешно собраны из автомата.");
                }
                else
                {
                    Console.WriteLine("В автомате не было денег.");
                }
            }
            else
            {
                Console.WriteLine("Операция отменена.");
            }

            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        private void AddMoneyToMachine()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПОПОЛНЕНИЕ АВТОМАТА МОНЕТАМИ ===");

                Console.WriteLine("Доступные номиналы монет:");
                Console.WriteLine("1. 1 рубль");
                Console.WriteLine("2. 2 рубля");
                Console.WriteLine("3. 5 рублей");
                Console.WriteLine("4. 10 рублей");
                Console.WriteLine("5. Вернуться в админ-меню");

                CoinType coinType;
                while (true)
                {
                    Console.Write("\nВыберите номинал (1-5): ");
                    string? choice = Console.ReadLine()?.Trim();

                    switch (choice)
                    {
                        case "1":
                            coinType = CoinType.One;
                            break;
                        case "2":
                            coinType = CoinType.Two;
                            break;
                        case "3":
                            coinType = CoinType.Five;
                            break;
                        case "4":
                            coinType = CoinType.Ten;
                            break;
                        case "5":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            continue;
                    }
                    break;
                }

                while (true)
                {
                    Console.Write($"Введите количество монет номиналом {(int)coinType} руб.: ");
                    if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                    {
                        Console.WriteLine("Неверное количество. Количество должно быть положительным числом. Попробуйте снова.");
                        continue;
                    }

                    _vendingMachine.AddMoneyToMachine(coinType, count);
                    decimal addedAmount = count * (int)coinType;

                    Console.WriteLine($"\nУспешно добавлено {count} монет номиналом {(int)coinType} руб.");
                    Console.WriteLine($"Общая добавленная сумма: {addedAmount:F0} руб.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
                }
            }
        }

        private void ShowMachineStatus()
        {
            Console.Clear();
            Console.WriteLine("=== СОСТОЯНИЕ АВТОМАТА ===");

            Console.WriteLine("ТОВАРЫ:");
            var products = _vendingMachine.GetProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("Товары отсутствуют.");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"  {product}");
                }
            }

            Console.WriteLine("\nДЕНЬГИ В АВТОМАТЕ:");
            Console.WriteLine($"  {_vendingMachine.GetMachineMoneyInfo()}");

            Console.WriteLine("\nВСТАВЛЕННЫЕ ПОЛЬЗОВАТЕЛЕМ ДЕНЬГИ:");
            Console.WriteLine($"  {_vendingMachine.GetInsertedMoneyInfo()}");

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}