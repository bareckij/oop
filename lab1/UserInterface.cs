using System;
using System.Collections.Generic;

namespace lab1
{
    public class UserInterface
    {
        private readonly VendingMachine _vendingMachine;
        private readonly AdminPanel _adminPanel;

        public UserInterface(VendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine ?? throw new ArgumentNullException(nameof(vendingMachine));
            _adminPanel = new AdminPanel(_vendingMachine);
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в вендинговый автомат!");
            
            while (true)
            {
                ShowMainMenu();
                string? input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                switch (input.Trim())
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        InsertMoney();
                        break;
                    case "3":
                        BuyProduct();
                        break;
                    case "4":
                        ShowInsertedMoney();
                        break;
                    case "5":
                        CancelTransaction();
                        break;
                    case "6":
                        AccessAdminPanel();
                        break;
                    case "7":
                        Console.WriteLine("Спасибо за использование вендингового автомата!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        PauseForUser();
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║         ВЕНДИНГОВЫЙ АВТОМАТ          ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1. Показать товары                   ║");
            Console.WriteLine("║ 2. Вставить монеты                   ║");
            Console.WriteLine("║ 3. Купить товар                      ║");
            Console.WriteLine("║ 4. Показать внесенные деньги         ║");
            Console.WriteLine("║ 5. Отменить операцию (вернуть деньги)║");
            Console.WriteLine("║ 6. Режим администратора              ║");
            Console.WriteLine("║ 7. Выход                             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("Выберите действие (1-7): ");
        }

        private void ShowProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ДОСТУПНЫЕ ТОВАРЫ ===");
            
            var products = _vendingMachine.GetProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("Товары отсутствуют.");
            }
            else
            {
                Console.WriteLine($"{"Код",-4} {"Название",-20} {"Цена (руб.)",-12} {"Статус",-15}");
                Console.WriteLine(new string('-', 55));
                
                foreach (var product in products)
                {
                    string status = product.IsAvailable ? $"В наличии ({product.Quantity} шт.)" : "Нет в наличии";
                    string priceColor = product.IsAvailable ? "" : "[НЕТ В НАЛИЧИИ]";
                    Console.WriteLine($"{product.Code,-4} {product.Name,-20} {product.Price,-12:F0} {status,-15}");
                }
            }

            PauseForUser();
        }

        private void InsertMoney()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВСТАВКА МОНЕТ ===");
                Console.WriteLine("Доступные номиналы:");
                Console.WriteLine("1. 1 рубль");
                Console.WriteLine("2. 2 рубля");
                Console.WriteLine("3. 5 рублей");
                Console.WriteLine("4. 10 рублей");
                Console.WriteLine("5. Вернуться в главное меню");
                
                Console.WriteLine($"\nТекущая сумма: {_vendingMachine.GetInsertedAmount():F0} руб.");
                Console.Write("Выберите номинал (1-5): ");

                string? input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                CoinType coinType;
                switch (input)
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
                        PauseForUser();
                        continue;
                }

                while (true)
                {
                    Console.Write($"Введите количество монет номиналом {(int)coinType} руб.: ");
                    if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
                    {
                        _vendingMachine.InsertCoin(coinType, count);
                        decimal addedAmount = count * (int)coinType;
                        Console.WriteLine($"Добавлено {addedAmount:F0} руб.");
                        Console.WriteLine($"Общая сумма: {_vendingMachine.GetInsertedAmount():F0} руб.");
                        Console.WriteLine("\nМонеты добавлены! Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверное количество монет. Введите положительное число.");
                    }
                }
            }
        }

        private void BuyProduct()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПОКУПКА ТОВАРА ===");

                decimal insertedAmount = _vendingMachine.GetInsertedAmount();
                if (insertedAmount <= 0)
                {
                    Console.WriteLine("Сначала вставьте монеты!");
                    PauseForUser();
                    return;
                }

                Console.WriteLine($"Внесено денег: {insertedAmount:F0} руб.");
                Console.WriteLine("\nДоступные товары:");

                var products = _vendingMachine.GetProducts();
                bool hasAvailableProducts = false;

                foreach (var product in products)
                {
                    if (product.IsAvailable)
                    {
                        hasAvailableProducts = true;
                        string canBuy = insertedAmount >= product.Price ? "[МОЖНО КУПИТЬ]" : "[НЕ ХВАТАЕТ ДЕНЕГ]";
                        Console.WriteLine($"{product.Code}: {product.Name} - {product.Price:F0} руб. {canBuy}");
                    }
                }

                if (!hasAvailableProducts)
                {
                    Console.WriteLine("Нет доступных товаров.");
                    PauseForUser();
                    return;
                }

                Console.WriteLine("\n0. Вернуться в главное меню");
                Console.Write("\nВведите код товара: ");
                string? productCode = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(productCode))
                {
                    Console.WriteLine("Код товара не может быть пустым. Попробуйте снова.");
                    PauseForUser();
                    continue;
                }

                if (productCode == "0")
                    return;

                var result = _vendingMachine.PurchaseProduct(productCode);
                
                Console.WriteLine();
                Console.WriteLine(result.Message);

                if (result.Success)
                {
                    if (result.Change > 0 && result.ChangeCoins != null)
                    {
                        Console.WriteLine($"Ваша сдача: {result.Change:F0} руб.");
                        Console.WriteLine("Выданные монеты:");
                        foreach (var coin in result.ChangeCoins)
                        {
                            if (coin.Value > 0)
                            {
                                Console.WriteLine($"  {coin.Value} x {(int)coin.Key} руб.");
                            }
                        }
                    }
                    Console.WriteLine("\nСпасибо за покупку! Не забудьте забрать товар.");
                    PauseForUser();
                    return;
                }
                else
                {
                    Console.WriteLine("\nПопробуйте снова или выберите другой товар.");
                    PauseForUser();
                }
            }
        }

        private void ShowInsertedMoney()
        {
            Console.Clear();
            Console.WriteLine("=== ВНЕСЕННЫЕ ДЕНЬГИ ===");
            Console.WriteLine(_vendingMachine.GetInsertedMoneyInfo());
            PauseForUser();
        }

        private void CancelTransaction()
        {
            Console.Clear();
            Console.WriteLine("=== ОТМЕНА ОПЕРАЦИИ ===");

            decimal insertedAmount = _vendingMachine.GetInsertedAmount();
            if (insertedAmount <= 0)
            {
                Console.WriteLine("Нет вставленных денег для возврата.");
                PauseForUser();
                return;
            }

            Console.WriteLine($"Будет возвращено: {insertedAmount:F0} руб.");
            Console.Write("Вы уверены, что хотите отменить операцию? (да/нет): ");

            string? confirmation = Console.ReadLine()?.Trim().ToLower();
            if (confirmation == "да" || confirmation == "yes" || confirmation == "y")
            {
                var returnedCoins = _vendingMachine.CancelOperation();
                
                Console.WriteLine("\nОперация отменена. Возвращенные монеты:");
                foreach (var coin in returnedCoins)
                {
                    if (coin.Value > 0)
                    {
                        Console.WriteLine($"  {coin.Value} x {(int)coin.Key} руб.");
                    }
                }
                Console.WriteLine("Не забудьте забрать свои деньги!");
            }
            else
            {
                Console.WriteLine("Отмена операции отменена.");
            }

            PauseForUser();
        }

        private void AccessAdminPanel()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВХОД В АДМИН-ПАНЕЛЬ ===");
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("\nВведите пароль администратора (или 0 для выхода): ");
                
                string? password = ReadPassword();
                
                if (password == "0")
                    return;
                
                if (_vendingMachine.CheckAdminPassword(password))
                {
                    Console.WriteLine("\nДоступ разрешен!");
                    Console.WriteLine("Нажмите любую клавишу для входа в админ-панель...");
                    Console.ReadKey();
                    _adminPanel.ShowMainMenu();
                    return;
                }
                else
                {
                    Console.WriteLine("\nНеверный пароль! Попробуйте снова.");
                    PauseForUser();
                }
            }
        }

        private string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            
            do
            {
                key = Console.ReadKey(true);
                
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            
            return password;
        }

        private void PauseForUser()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}