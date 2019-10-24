using System.Collections.Generic;
using Entitas;

public class ItemReservationCompletionSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly Contexts _contexts;
    private readonly List<GameEntity> _toCleanUp;

    public ItemReservationCompletionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _toCleanUp = new List<GameEntity>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ItemReservationCompleted);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasItemReservation && entity.isItemReservationCompleted;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.isItem)
            {
                entity.isWillBeDestroyed = true;
            }

            var pos = entity.gridPosition.value;
            var itemType = entity.itemReservation.ItemType;
            var item = _contexts.game.CreateEntity();
            item.AddWillSpawnItem(itemType, pos, pos.y);
            item.isCreatedFromGenerator = true;

            _toCleanUp.Add(entity);
        }
    }

    public void Cleanup()
    {
        foreach (var entity in _toCleanUp)
        {
            entity.RemoveItemReservation();
            entity.isItemReservationCompleted = false;
            WaitHelper.Reduce(WaitType.CriticalAnimation);
        }

        _toCleanUp.Clear();
    }
}