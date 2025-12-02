namespace InventorySystem.Patterns;

public interface IItemState
{
    void Equip(Items.IItem item);
    void Use(Items.IItem item);
}

public class InInventoryState : IItemState
{
    public void Equip(Items.IItem item)
    {
        // transition logic handled by Inventory/Equipment system
    }

    public void Use(Items.IItem item)
    {
        // transition logic handled by Inventory system
    }
}

public class EquippedState : IItemState
{
    public void Equip(Items.IItem item)
    {
        // already equipped
    }

    public void Use(Items.IItem item)
    {
        // cannot directly use equipped (for weapons/armor)
    }
}

public class ConsumedState : IItemState
{
    public void Equip(Items.IItem item)
    {
        // cannot equip consumed item
    }

    public void Use(Items.IItem item)
    {
        // already used
    }
}
