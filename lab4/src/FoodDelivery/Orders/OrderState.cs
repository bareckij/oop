namespace FoodDelivery.Domain.Orders;

public abstract class OrderState
{
    public abstract OrderStatus Status { get; }
    public virtual OrderState MoveToPreparing() => this;
    public virtual OrderState MoveToDelivering() => this;
    public virtual OrderState MoveToCompleted() => this;
}

public class CreatedState : OrderState
{
    public override OrderStatus Status => OrderStatus.Created;

    public override OrderState MoveToPreparing() => new PreparingState();
}

public class PreparingState : OrderState
{
    public override OrderStatus Status => OrderStatus.Preparing;

    public override OrderState MoveToDelivering() => new DeliveringState();
}

public class DeliveringState : OrderState
{
    public override OrderStatus Status => OrderStatus.Delivering;

    public override OrderState MoveToCompleted() => new CompletedState();
}

public class CompletedState : OrderState
{
    public override OrderStatus Status => OrderStatus.Completed;
}
