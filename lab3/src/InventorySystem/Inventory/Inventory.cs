using InventorySystem.Items;
using InventorySystem.Patterns;

namespace InventorySystem.Inventory;

public class Inventory
{
    private readonly List<IItem> _items = new();

    public IReadOnlyCollection<IItem> Items => _items.AsReadOnly();

    public void AddItem(IItem item)
    {
        _items.Add(item);
    }

    public bool RemoveItem(IItem item)
    {
        return _items.Remove(item);
    }
}
