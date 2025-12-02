using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    public class VendingMachine
    {
        private readonly Dictionary<string, Product> _products;
        private readonly MoneyStorage _machineStorage;  
        private MoneyStorage _insertedMoney;   
        private readonly string _adminPassword = "1111";

        public VendingMachine()
        {
            _products = new Dictionary<string, Product>();
            _machineStorage = new MoneyStorage();
            _insertedMoney = new MoneyStorage();
            
            InitializeProducts();
            InitializeMachineStorage();
        }

        private void InitializeProducts()
        {
            _products["A1"] = new Product("Кока-Кола", 76m, 10, "A1");
            _products["A2"] = new Product("Пепси", 73m, 8, "A2");
            _products["B1"] = new Product("Сникерс", 90m, 15, "B1");
            _products["B2"] = new Product("Кит-Кат", 85m, 12, "B2");
            _products["C1"] = new Product("Чипсы Лейс", 96m, 20, "C1");
            _products["C2"] = new Product("Орешки", 120m, 6, "C2");
        }

        private void InitializeMachineStorage()
        {
            _machineStorage.AddCoin(CoinType.One, 50);
            _machineStorage.AddCoin(CoinType.Two, 30);
            _machineStorage.AddCoin(CoinType.Five, 20);
            _machineStorage.AddCoin(CoinType.Ten, 15);
        }

        public List<Product> GetProducts()
        {
            return _products.Values.ToList();
        }

        public Product? GetProduct(string code)
        {
            return _products.ContainsKey(code.ToUpper()) ? _products[code.ToUpper()] : null;
        }

        public void InsertCoin(CoinType coinType, int count = 1)
        {
            _insertedMoney.AddCoin(coinType, count);
        }

        public decimal GetInsertedAmount()
        {
            return _insertedMoney.GetTotalAmount();
        }

        public PurchaseResult PurchaseProduct(string productCode)
        {
            var product = GetProduct(productCode);
            if (product == null)
                return new PurchaseResult(false, "Товар с таким кодом не найден");

            if (!product.IsAvailable)
                return new PurchaseResult(false, "Товар закончился");

            decimal insertedAmount = GetInsertedAmount();
            if (insertedAmount < product.Price)
                return new PurchaseResult(false, $"Недостаточно средств. Нужно: {product.Price:F0} руб., внесено: {insertedAmount:F0} руб.");

            decimal changeAmount = insertedAmount - product.Price;


            TransferInsertedMoneyToMachine();


            Dictionary<CoinType, int>? change = null;
            if (changeAmount > 0)
            {
                change = _machineStorage.CalculateChange(changeAmount);
                if (change == null)
                {
    
                    TransferMachineMoneyToInserted();
                    return new PurchaseResult(false, "Не могу выдать сдачу. Попробуйте внести точную сумму.");
                }
            }


            if (product.Purchase())
            {

                if (changeAmount > 0 && change != null)
                {
                    _machineStorage.GiveChange(changeAmount);
                }

                _insertedMoney = new MoneyStorage();
                
                return new PurchaseResult(true, $"Товар '{product.Name}' успешно куплен!", 
                    product, changeAmount, change);
            }

            return new PurchaseResult(false, "Ошибка при покупке товара");
        }

        public Dictionary<CoinType, int> CancelOperation()
        {
            var returnedMoney = new Dictionary<CoinType, int>
            {
                { CoinType.One, _insertedMoney.GetCoinCount(CoinType.One) },
                { CoinType.Two, _insertedMoney.GetCoinCount(CoinType.Two) },
                { CoinType.Five, _insertedMoney.GetCoinCount(CoinType.Five) },
                { CoinType.Ten, _insertedMoney.GetCoinCount(CoinType.Ten) }
            };


            foreach (var coinType in Enum.GetValues<CoinType>())
            {
                _insertedMoney.RemoveCoin(coinType, _insertedMoney.GetCoinCount(coinType));
            }

            return returnedMoney;
        }

        private void TransferInsertedMoneyToMachine()
        {
            foreach (var coinType in Enum.GetValues<CoinType>())
            {
                int count = _insertedMoney.GetCoinCount(coinType);
                if (count > 0)
                {
                    _machineStorage.AddCoin(coinType, count);
                }
            }
        }

        private void TransferMachineMoneyToInserted()
        {

        }

        public bool CheckAdminPassword(string password)
        {
            return password == _adminPassword;
        }

        public void AddProduct(Product product)
        {
            _products[product.Code] = product;
        }

        public bool RestockProduct(string code, int amount)
        {
            var product = GetProduct(code);
            if (product == null)
                return false;

            product.AddStock(amount);
            return true;
        }

        public decimal CollectMoney()
        {
            return _machineStorage.CollectAllMoney();
        }

        public void AddMoneyToMachine(CoinType coinType, int count)
        {
            _machineStorage.AddCoin(coinType, count);
        }

        public string GetMachineMoneyInfo()
        {
            return _machineStorage.ToString();
        }

        public string GetInsertedMoneyInfo()
        {
            return _insertedMoney.ToString();
        }
    }

    public class PurchaseResult
    {
        public bool Success { get; }
        public string Message { get; }
        public Product? Product { get; }
        public decimal Change { get; }
        public Dictionary<CoinType, int>? ChangeCoins { get; }

        public PurchaseResult(bool success, string message, Product? product = null, 
            decimal change = 0, Dictionary<CoinType, int>? changeCoins = null)
        {
            Success = success;
            Message = message;
            Product = product;
            Change = change;
            ChangeCoins = changeCoins;
        }
    }
}