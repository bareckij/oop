using InventorySystem.Patterns;

namespace InventorySystem.Items;

public class Weapon : IEquipable
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ItemRarity Rarity { get; }
    public int Damage { get; }
    public EquipmentSlot Slot { get; }

    public Weapon(string id, string name, string description, ItemRarity rarity, int damage, EquipmentSlot slot)
    {
        Id = id;
        Name = name;
        Description = description;
        Rarity = rarity;
        Damage = damage;
        Slot = slot;
    }
}

public class Armor : IEquipable
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ItemRarity Rarity { get; }
    public int Defense { get; }
    public EquipmentSlot Slot { get; }

    public Armor(string id, string name, string description, ItemRarity rarity, int defense, EquipmentSlot slot)
    {
        Id = id;
        Name = name;
        Description = description;
        Rarity = rarity;
        Defense = defense;
        Slot = slot;
    }
}

public class Potion : IConsumable
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ItemRarity Rarity { get; }
    public IUseStrategy UseStrategy { get; }

    public Potion(string id, string name, string description, ItemRarity rarity, IUseStrategy useStrategy)
    {
        Id = id;
        Name = name;
        Description = description;
        Rarity = rarity;
        UseStrategy = useStrategy;
    }
}

public class QuestItem : IItem
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ItemRarity Rarity { get; }

    public QuestItem(string id, string name, string description, ItemRarity rarity)
    {
        Id = id;
        Name = name;
        Description = description;
        Rarity = rarity;
    }
}
