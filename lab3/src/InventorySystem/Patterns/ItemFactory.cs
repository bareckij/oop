using InventorySystem.Items;

namespace InventorySystem.Patterns;

public interface IItemFactory
{
    Weapon CreateWeapon(string name, int damage, ItemRarity rarity = ItemRarity.Common);
    Armor CreateArmor(string name, int defense, EquipmentSlot slot, ItemRarity rarity = ItemRarity.Common);
    Potion CreateHealingPotion(string name, int healAmount, ItemRarity rarity = ItemRarity.Common);
}

public class WarriorItemFactory : IItemFactory
{
    public Weapon CreateWeapon(string name, int damage, ItemRarity rarity = ItemRarity.Common)
    {
        string id = Guid.NewGuid().ToString();
        return new Weapon(id, name, "Warrior weapon", rarity, damage, EquipmentSlot.Weapon);
    }

    public Armor CreateArmor(string name, int defense, EquipmentSlot slot, ItemRarity rarity = ItemRarity.Common)
    {
        string id = Guid.NewGuid().ToString();
        return new Armor(id, name, "Warrior armor", rarity, defense, slot);
    }

    public Potion CreateHealingPotion(string name, int healAmount, ItemRarity rarity = ItemRarity.Common)
    {
        string id = Guid.NewGuid().ToString();
        return new Potion(id, name, "Healing potion", rarity, new HealUseStrategy(healAmount));
    }
}

public class MageItemFactory : IItemFactory
{
    public Weapon CreateWeapon(string name, int damage, ItemRarity rarity = ItemRarity.Common)
        => new(Guid.NewGuid().ToString(), name, "Mage staff", rarity, damage, EquipmentSlot.Weapon);

    public Armor CreateArmor(string name, int defense, EquipmentSlot slot, ItemRarity rarity = ItemRarity.Common)
        => new(Guid.NewGuid().ToString(), name, "Mage robe", rarity, defense, slot);

    public Potion CreateHealingPotion(string name, int healAmount, ItemRarity rarity = ItemRarity.Common)
        => new(Guid.NewGuid().ToString(), name, "Mana potion", rarity, new BuffAttackStrategy(healAmount));
}
