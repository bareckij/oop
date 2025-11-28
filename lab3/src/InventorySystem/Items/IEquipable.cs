namespace InventorySystem.Items;

public interface IEquipable : IItem
{
    EquipmentSlot Slot { get; }
}

public enum EquipmentSlot
{
    Weapon,
    Head,
    Chest,
    Legs,
    Accessory
}
