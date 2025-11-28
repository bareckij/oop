using InventorySystem.Items;

namespace InventorySystem.Upgrades;

public class UpgradeService
{
    private readonly IUpgradeStrategy _strategy;

    public UpgradeService(IUpgradeStrategy strategy)
    {
        _strategy = strategy;
    }

    public IItem Upgrade(IItem baseItem, IItem material)
        => _strategy.Upgrade(baseItem, material);
}
