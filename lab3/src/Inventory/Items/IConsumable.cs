using InventorySystem.Patterns;

namespace InventorySystem.Items;

public interface IConsumable : IItem
{
    IUseStrategy UseStrategy { get; }
}
