using InventorySystem.Items;

namespace InventorySystem.Patterns;

public interface IItemBuilder
{
    void Reset();
    void SetBase(string id, string name, string description, ItemRarity rarity);
    void SetWeaponStats(int damage);
    void SetArmorStats(int defense, EquipmentSlot slot);
    void SetConsumable(IUseStrategy strategy);
    IItem Build();
}

public class WeaponBuilder : IItemBuilder
{
    private string _id = "";
    private string _name = "";
    private string _description = "";
    private ItemRarity _rarity = ItemRarity.Common;
    private int _damage;

    public void Reset()
    {
        _id = "";
        _name = "";
        _description = "";
        _rarity = ItemRarity.Common;
        _damage = 0;
    }

    public void SetBase(string id, string name, string description, ItemRarity rarity)
    {
        _id = id;
        _name = name;
        _description = description;
        _rarity = rarity;
    }

    public void SetWeaponStats(int damage)
    {
        _damage = damage;
    }

    public void SetArmorStats(int defense, EquipmentSlot slot) { }

    public void SetConsumable(IUseStrategy strategy) { }

    public IItem Build()
    {
        return new Weapon(_id, _name, _description, _rarity, _damage, EquipmentSlot.Weapon);
    }
}

public class ArmorBuilder : IItemBuilder
{
    private string _id = "";
    private string _name = "";
    private string _description = "";
    private ItemRarity _rarity = ItemRarity.Common;
    private int _defense;
    private EquipmentSlot _slot = EquipmentSlot.Chest;

    public void Reset()
    {
        _id = "";
        _name = "";
        _description = "";
        _rarity = ItemRarity.Common;
        _defense = 0;
        _slot = EquipmentSlot.Chest;
    }

    public void SetBase(string id, string name, string description, ItemRarity rarity)
    {
        _id = id;
        _name = name;
        _description = description;
        _rarity = rarity;
    }

    public void SetWeaponStats(int damage) { }

    public void SetArmorStats(int defense, EquipmentSlot slot)
    {
        _defense = defense;
        _slot = slot;
    }

    public void SetConsumable(IUseStrategy strategy) { }

    public IItem Build()
    {
        return new Armor(_id, _name, _description, _rarity, _defense, _slot);
    }
}

public class ItemDirector
{
    private readonly IItemBuilder _builder;

    public ItemDirector(IItemBuilder builder)
    {
        _builder = builder;
    }

    public IItem CreateBasicSword(string id)
    {
        _builder.Reset();
        _builder.SetBase(id, "Sword", "Basic sword", ItemRarity.Common);
        _builder.SetWeaponStats(10);
        return _builder.Build();
    }

    public IItem CreateBasicArmor(string id)
    {
        _builder.Reset();
        _builder.SetBase(id, "Armor", "Basic armor", ItemRarity.Common);
        _builder.SetArmorStats(5, EquipmentSlot.Chest);
        return _builder.Build();
    }
}
