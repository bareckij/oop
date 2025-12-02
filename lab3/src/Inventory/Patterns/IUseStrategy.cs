using InventorySystem.Inventory;

namespace InventorySystem.Patterns;

public interface IUseStrategy
{
    void Use(PlayerContext player, Items.IItem item);
}

public class HealUseStrategy : IUseStrategy
{
    private readonly int _amount;

    public HealUseStrategy(int amount)
    {
        _amount = amount;
    }

    public void Use(PlayerContext player, Items.IItem item)
    {
        player.Heal(_amount);
    }
}

public class BuffAttackStrategy : IUseStrategy
{
    private readonly int _amount;

    public BuffAttackStrategy(int amount)
    {
        _amount = amount;
    }

    public void Use(PlayerContext player, Items.IItem item)
    {
        player.BuffAttack(_amount);
    }
}
