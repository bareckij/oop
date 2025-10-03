using System;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var vendingMachine = new VendingMachine();
                var userInterface = new UserInterface(vendingMachine);
                userInterface.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла критическая ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
