using InventorySystem.Items;
using InventorySystem.Patterns;

namespace InventorySystem.Inventory;

public class InventoryService
{
    private readonly Inventory _inventory;
    private readonly EquipmentSet _equipment;
    private readonly PlayerContext _player;

    public InventoryService(Inventory inventory, EquipmentSet equipment, PlayerContext player)
    {
        _inventory = inventory;
        _equipment = equipment;
        _player = player;
    }

    public void AddItem(IItem item) => _inventory.AddItem(item);

    public bool RemoveItem(IItem item) => _inventory.RemoveItem(item);

    public void EquipItem(IEquipable item)
    {
        if (_inventory.Items.Contains(item))
        {
            _equipment.Equip(item);
        }
    }

    public void UseConsumable(IConsumable item)
    {
        if (_inventory.Items.Contains(item))
        {
            item.UseStrategy.Use(_player, item);
            _inventory.RemoveItem(item);
        }
    }

    public IReadOnlyCollection<IItem> GetItems() => _inventory.Items;

    public IEquipable? GetEquipped(EquipmentSlot slot) => _equipment.Get(slot);
}
