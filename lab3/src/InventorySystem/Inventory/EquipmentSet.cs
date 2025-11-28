using InventorySystem.Items;

namespace InventorySystem.Inventory;

public class EquipmentSet
{
    private readonly Dictionary<EquipmentSlot, IEquipable?> _slots = new();

    public EquipmentSet()
    {
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            _slots[slot] = null;
        }
    }

    public IEquipable? Get(EquipmentSlot slot) => _slots[slot];

    public void Equip(IEquipable item)
    {
        _slots[item.Slot] = item;
    }

    public void Unequip(EquipmentSlot slot)
    {
        _slots[slot] = null;
    }
}
