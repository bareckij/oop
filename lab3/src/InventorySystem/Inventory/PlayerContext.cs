namespace InventorySystem.Inventory;

public class PlayerContext
{
    public int Health { get; private set; } = 100;
    public int Attack { get; private set; } = 10;

    public void Heal(int amount)
    {
        Health += amount;
    }

    public void BuffAttack(int amount)
    {
        Attack += amount;
    }
}
