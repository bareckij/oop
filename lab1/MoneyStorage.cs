using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    public enum CoinType
    {
        One = 1,       
        Two = 2,       
        Five = 5,     
        Ten = 10       
    }

    public class MoneyStorage
    {
        private readonly Dictionary<CoinType, int> _coins;

        public MoneyStorage()
        {
            _coins = new Dictionary<CoinType, int>
            {
                { CoinType.One, 0 },
                { CoinType.Two, 0 },
                { CoinType.Five, 0 },
                { CoinType.Ten, 0 }
            };
        }

        public void AddCoin(CoinType coinType, int count = 1)
        {
            if (count < 0)
                throw new ArgumentException("Количество монет не может быть отрицательным");

            _coins[coinType] += count;
        }

        public bool RemoveCoin(CoinType coinType, int count)
        {
            if (count < 0)
                throw new ArgumentException("Количество монет не может быть отрицательным");

            if (_coins[coinType] < count)
                return false;

            _coins[coinType] -= count;
            return true;
        }

        public int GetCoinCount(CoinType coinType)
        {
            return _coins[coinType];
        }

        public decimal GetTotalAmount()
        {
            return _coins.Sum(coin => (decimal)coin.Key * coin.Value);
        }

        public Dictionary<CoinType, int>? CalculateChange(decimal amount)
        {
            if (amount <= 0)
                return new Dictionary<CoinType, int>();

            var change = new Dictionary<CoinType, int>
            {
                { CoinType.Ten, 0 },
                { CoinType.Five, 0 },
                { CoinType.Two, 0 },
                { CoinType.One, 0 }
            };

            var availableCoins = new Dictionary<CoinType, int>(_coins);
            decimal remainingAmount = amount;


            foreach (var coinType in new[] { CoinType.Ten, CoinType.Five, CoinType.Two, CoinType.One })
            {
                int coinValue = (int)coinType;
                int neededCoins = (int)(remainingAmount / coinValue);
                int availableCoinsCount = availableCoins[coinType];

                if (neededCoins > 0 && availableCoinsCount > 0)
                {
                    int coinsToUse = Math.Min(neededCoins, availableCoinsCount);
                    change[coinType] = coinsToUse;
                    remainingAmount -= coinsToUse * coinValue;
                    availableCoins[coinType] -= coinsToUse;
                }
            }

            if (remainingAmount > 0.01m)
                return null;

            return change;
        }

        public Dictionary<CoinType, int>? GiveChange(decimal amount)
        {
            var change = CalculateChange(amount);
            if (change == null)
                return null;


            foreach (var coinPair in change)
            {
                _coins[coinPair.Key] -= coinPair.Value;
            }

            return change;
        }

        public decimal CollectAllMoney()
        {
            decimal total = GetTotalAmount();
            foreach (var key in _coins.Keys.ToList())
            {
                _coins[key] = 0;
            }
            return total;
        }

        public string GetCoinsInfo()
        {
            var info = new List<string>();
            foreach (var coin in _coins)
            {
                if (coin.Value > 0)
                {
                    info.Add($"{coin.Value} x {(int)coin.Key} руб.");
                }
            }
            return info.Count > 0 ? string.Join(", ", info) : "Нет монет";
        }

        public override string ToString()
        {
            return $"Общая сумма: {GetTotalAmount():F0} руб. ({GetCoinsInfo()})";
        }
    }
}