using System;

namespace lab1
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
        public string Code { get; set; }

        public Product(string name, decimal price, int quantity, string code)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price >= 0 ? price : throw new ArgumentException("Цена не может быть отрицательной");
            Quantity = quantity >= 0 ? quantity : throw new ArgumentException("Количество не может быть отрицательным");
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public bool IsAvailable => Quantity > 0;

        public bool Purchase()
        {
            if (!IsAvailable)
                return false;

            Quantity--;
            return true;
        }

        public void AddStock(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Количество добавляемого товара не может быть отрицательным");
            
            Quantity += amount;
        }

        public override string ToString()
        {
            string status = IsAvailable ? "В наличии" : "Нет в наличии";
            return $"{Code}: {Name} - {Price:F0} руб. ({Quantity} шт.) [{status}]";
        }
    }
}