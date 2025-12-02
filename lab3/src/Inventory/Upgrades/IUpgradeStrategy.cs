using InventorySystem.Items;

namespace InventorySystem.Upgrades;

public interface IUpgradeStrategy
{
    IItem Upgrade(IItem baseItem, IItem material);
}

public class SimpleLevelUpgradeStrategy : IUpgradeStrategy
{
    public IItem Upgrade(IItem baseItem, IItem material)
    {
        if (baseItem is Weapon weapon && material is Weapon materialWeapon)
        {
            int newDamage = weapon.Damage + materialWeapon.Damage / 2;
            return new Weapon(weapon.Id, weapon.Name, weapon.Description, weapon.Rarity, newDamage, weapon.Slot);
        }

        if (baseItem is Armor armor && material is Armor materialArmor)
        {
            int newDefense = armor.Defense + materialArmor.Defense / 2;
            return new Armor(armor.Id, armor.Name, armor.Description, armor.Rarity, newDefense, armor.Slot);
        }

        throw new InvalidOperationException("Unsupported upgrade combination");
    }
}
