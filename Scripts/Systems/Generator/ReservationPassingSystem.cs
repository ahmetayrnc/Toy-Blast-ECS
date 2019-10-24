using System;
using System.Collections.Generic;
using Entitas;

public class ReservationPassingSystem : ReactiveSystem<GameEntity>
{
    public ReservationPassingSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillBeDestroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isItem && entity.hasItemReservation && entity.isWillBeDestroyed &&
               entity.isItemReservationCompleted == false;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var cell = Contexts.sharedInstance.game.GetEntityWithCellId(new Tuple<int, int>(entity.gridPosition.value.x,
                entity.gridPosition.value.y));
            var reservationId = entity.itemReservation.ReservationId;
            var itemType = entity.itemReservation.ItemType;
            entity.RemoveItemReservation();
            cell.AddItemReservation(reservationId, cell.id.Value, itemType);
        }
    }
}