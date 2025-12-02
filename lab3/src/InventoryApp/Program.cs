using InventorySystem.Inventory;
using InventorySystem.Items;
using InventorySystem.Patterns;

namespace InventoryApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var inventory = new Inventory();
        var equipment = new EquipmentSet();
        var player = new PlayerContext();
        IItemFactory factory = new WarriorItemFactory();
        var service = new InventoryService(inventory, equipment, player);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Инвентарь RPG ===");
            Console.WriteLine($"Здоровье: {player.Health}, Атака: {player.Attack}");
            Console.WriteLine();
            Console.WriteLine("1. Добавить оружие");
            Console.WriteLine("2. Добавить броню");
            Console.WriteLine("3. Добавить зелье лечения");
            Console.WriteLine("4. Добавить квестовый предмет");
            Console.WriteLine("5. Экипировать предмет");
            Console.WriteLine("6. Использовать зелье");
            Console.WriteLine("7. Показать инвентарь");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите команду: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddWeapon(factory, service);
                    break;
                case "2":
                    AddArmor(factory, service);
                    break;
                case "3":
                    AddPotion(factory, service);
                    break;
                case "4":
                    AddQuestItem(service);
                    break;
                case "5":
                    EquipItem(service, equipment);
                    break;
                case "6":
                    UsePotion(service, player);
                    break;
                case "7":
                    ShowInventory(service, equipment);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неизвестная команда. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void AddWeapon(IItemFactory factory, InventoryService service)
    {
        Console.Write("Название оружия: ");
        var name = Console.ReadLine() ?? "Меч";
        Console.Write("Урон: ");
        int.TryParse(Console.ReadLine(), out var damage);
        if (damage <= 0) damage = 10;
        var rarity = AskRarity();
        var weapon = factory.CreateWeapon(name, damage, rarity);
        service.AddItem(weapon);
        Console.WriteLine($"Добавлено оружие: {weapon.Name} (урон {damage}, редкость {weapon.Rarity})");
        Console.ReadKey();
    }

    private static void AddArmor(IItemFactory factory, InventoryService service)
    {
        Console.Write("Название брони: ");
        var name = Console.ReadLine() ?? "Броня";
        Console.Write("Защита: ");
        int.TryParse(Console.ReadLine(), out var defense);
        if (defense <= 0) defense = 5;
        var slot = AskArmorSlot();
        var rarity = AskRarity();
        var armor = factory.CreateArmor(name, defense, slot, rarity);
        service.AddItem(armor);
        Console.WriteLine($"Добавлена броня: {armor.Name} (защита {defense}, слот {armor.Slot}, редкость {armor.Rarity})");
        Console.ReadKey();
    }

    private static void AddPotion(IItemFactory factory, InventoryService service)
    {
        Console.Write("Название зелья: ");
        var name = Console.ReadLine() ?? "Зелье";
        Console.Write("Восстанавливаемое здоровье: ");
        int.TryParse(Console.ReadLine(), out var heal);
        if (heal <= 0) heal = 20;
        var rarity = AskRarity();
        var potion = factory.CreateHealingPotion(name, heal, rarity);
        service.AddItem(potion);
        Console.WriteLine($"Добавлено зелье: {potion.Name} (лечение {heal}, редкость {potion.Rarity})");
        Console.ReadKey();
    }

    private static void AddQuestItem(InventoryService service)
    {
        Console.Write("Название квестового предмета: ");
        var name = Console.ReadLine() ?? "Квестовый предмет";
        var rarity = AskRarity();
        var item = new QuestItem(Guid.NewGuid().ToString(), name, "Важный квестовый предмет", rarity);
        service.AddItem(item);
        Console.WriteLine($"Добавлен квестовый предмет: {item.Name}");
        Console.ReadKey();
    }

    private static ItemRarity AskRarity()
    {
        Console.WriteLine("Выберите редкость:");
        Console.WriteLine("1. Обычный");
        Console.WriteLine("2. Редкий");
        Console.WriteLine("3. Эпический");
        Console.WriteLine("4. Легендарный");
        Console.Write("Номер (по умолчанию 1): ");
        int.TryParse(Console.ReadLine(), out var choice);
        return choice switch
        {
            2 => ItemRarity.Rare,
            3 => ItemRarity.Epic,
            4 => ItemRarity.Legendary,
            _ => ItemRarity.Common
        };
    }

    private static EquipmentSlot AskArmorSlot()
    {
        Console.WriteLine("Выберите слот брони:");
        Console.WriteLine("1. Голова");
        Console.WriteLine("2. Грудь");
        Console.WriteLine("3. Ноги");
        Console.Write("Номер (по умолчанию 2 - грудь): ");
        int.TryParse(Console.ReadLine(), out var choice);
        return choice switch
        {
            1 => EquipmentSlot.Head,
            3 => EquipmentSlot.Legs,
            _ => EquipmentSlot.Chest
        };
    }

    private static void EquipItem(InventoryService service, EquipmentSet equipment)
    {
        var items = service.GetItems().OfType<IEquipable>().ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("Нет экипируемых предметов.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Выберите предмет для экипировки:");
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i].Name} (слот {items[i].Slot})");
        }
        Console.Write("Номер: ");
        int.TryParse(Console.ReadLine(), out var index);
        index--;
        if (index < 0 || index >= items.Count)
        {
            Console.WriteLine("Неверный выбор.");
            Console.ReadKey();
            return;
        }

        service.EquipItem(items[index]);
        Console.WriteLine($"Экипировано: {items[index].Name}");
        Console.ReadKey();
    }

    private static void UsePotion(InventoryService service, PlayerContext player)
    {
        var potions = service.GetItems().OfType<IConsumable>().ToList();
        if (potions.Count == 0)
        {
            Console.WriteLine("Нет зелий для использования.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Выберите зелье:");
        for (int i = 0; i < potions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {potions[i].Name}");
        }
        Console.Write("Номер: ");
        int.TryParse(Console.ReadLine(), out var index);
        index--;
        if (index < 0 || index >= potions.Count)
        {
            Console.WriteLine("Неверный выбор.");
            Console.ReadKey();
            return;
        }

        var beforeHealth = player.Health;
        service.UseConsumable(potions[index]);
        Console.WriteLine($"Зелье использовано. Здоровье: {beforeHealth} -> {player.Health}");
        Console.ReadKey();
    }

    private static void ShowInventory(InventoryService service, EquipmentSet equipment)
    {
        Console.WriteLine("=== Содержимое инвентаря ===");
        foreach (var item in service.GetItems())
        {
            string extra = item switch
            {
                Weapon w => $", урон: {w.Damage}",
                Armor a => $", защита: {a.Defense}, слот: {a.Slot}",
                Potion => ", зелье",
                QuestItem => ", квестовый предмет",
                _ => string.Empty
            };
            Console.WriteLine($"- {item.Name} [{item.Rarity}]{extra}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Экипировка ===");
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            var eq = equipment.Get(slot);
            Console.WriteLine($"{slot}: {(eq?.Name ?? "пусто")}");
        }
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }
}
