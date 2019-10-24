using System.Collections.Generic;
using Entitas;

public class CellItemReservationCompletionSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly Contexts _contexts;
    private readonly List<GameEntity> _forCleanUp;

    public CellItemReservationCompletionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _forCleanUp = new List<GameEntity>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CellItemReservationCompleted);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCellItemReservation && entity.isCellItemReservationCompleted;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var cellItemType = entity.cellItemReservation.CellItemType;
            var cellItem = _contexts.game.CreateEntity();
            cellItem.AddWillSpawnCellItem(cellItemType, entity.cellId.Value.Item1, entity.cellId.Value.Item2);
            cellItem.isCreatedFromGenerator = true;

            _forCleanUp.Add(entity);
        }
    }

    public void Cleanup()
    {
        foreach (var entity in _forCleanUp)
        {
            entity.isCellItemReservationCompleted = false;
            entity.RemoveCellItemReservation();
            WaitHelper.Reduce(WaitType.CriticalAnimation);
        }

        _forCleanUp.Clear();
    }
}