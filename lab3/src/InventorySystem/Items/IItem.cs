namespace InventorySystem.Items;

public interface IItem
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    ItemRarity Rarity { get; }
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
