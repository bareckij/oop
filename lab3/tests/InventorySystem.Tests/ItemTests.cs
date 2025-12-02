using InventorySystem.Inventory;
using InventorySystem.Items;
using InventorySystem.Patterns;
using InventorySystem.Upgrades;

namespace InventorySystem.Tests;

public class ItemTests
{
    [Fact]
    public void WarriorFactory_Creates_Weapon_With_Damage()
    {
        IItemFactory factory = new WarriorItemFactory();
        var sword = factory.CreateWeapon("Sword", 15);

        Assert.Equal("Sword", sword.Name);
        Assert.Equal(15, sword.Damage);
    }

    [Fact]
    public void ItemDirector_Builds_Basic_Sword()
    {
        var builder = new WeaponBuilder();
        var director = new ItemDirector(builder);

        var sword = director.CreateBasicSword("id-1");

        Assert.Equal("Sword", sword.Name);
        Assert.Equal(10, ((Weapon)sword).Damage);
    }

    [Fact]
    public void Upgrade_Increases_Weapon_Damage()
    {
        var baseWeapon = new Weapon("1", "Sword", "", ItemRarity.Common, 10, EquipmentSlot.Weapon);
        var material = new Weapon("2", "Sword+", "", ItemRarity.Common, 6, EquipmentSlot.Weapon);

        var strategy = new SimpleLevelUpgradeStrategy();
        var service = new UpgradeService(strategy);

        var upgraded = (Weapon)service.Upgrade(baseWeapon, material);

        Assert.Equal(13, upgraded.Damage);
    }
}
