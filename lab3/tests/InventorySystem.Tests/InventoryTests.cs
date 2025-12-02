using InventorySystem.Inventory;
using InventorySystem.Items;
using InventorySystem.Patterns;

namespace InventorySystem.Tests;

public class InventoryTests
{
    [Fact]
    public void Can_Add_And_Remove_Items()
    {
        var inventory = new InventorySystem.Inventory.Inventory();
        var item = new QuestItem("1", "Quest", "", ItemRarity.Common);

        inventory.AddItem(item);
        Assert.Contains(item, inventory.Items);

        inventory.RemoveItem(item);
        Assert.DoesNotContain(item, inventory.Items);
    }

    [Fact]
    public void Can_Equip_Weapon_From_Inventory()
    {
        var inventory = new InventorySystem.Inventory.Inventory();
        var equipment = new EquipmentSet();
        var player = new PlayerContext();
        var service = new InventoryService(inventory, equipment, player);

        var sword = new Weapon("1", "Sword", "", ItemRarity.Common, 10, EquipmentSlot.Weapon);

        service.AddItem(sword);
        service.EquipItem(sword);

        Assert.Same(sword, service.GetEquipped(EquipmentSlot.Weapon));
    }

    [Fact]
    public void Using_Potion_Heals_Player_And_Removes_Item()
    {
        var inventory = new InventorySystem.Inventory.Inventory();
        var equipment = new EquipmentSet();
        var player = new PlayerContext();
        var service = new InventoryService(inventory, equipment, player);

        var potion = new Potion("1", "Heal", "", ItemRarity.Common, new HealUseStrategy(20));

        service.AddItem(potion);
        var healthBefore = player.Health;

        service.UseConsumable(potion);

        Assert.Equal(healthBefore + 20, player.Health);
        Assert.DoesNotContain(potion, inventory.Items);
    }
}
