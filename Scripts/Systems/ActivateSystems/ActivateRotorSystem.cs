using System.Collections.Generic;
using Entitas;

public class ActivateRotorSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateRotorSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasItemType && Equals(entity.itemType.Value, ItemType.Rotor);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateRotor(entity);
        }
    }

    private void ActivateRotor(GameEntity entity)
    {
        entity.isWillBeDestroyed = true;

        CreateActivateRotors(entity);
    }

    private void CreateActivateRotors(GameEntity entity)
    {
        var removerId = IdHelper.GetNewRemoverId();
        var axis = entity.itemAxis.Value;
        var pos = entity.gridPosition.value;
        var id = removerId;

        switch (axis)
        {
            case Axis.Horizontal:
                CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Left, pos, id);
                CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Right, pos, id);
                break;
            
            case Axis.Vertical:
                CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Up, pos, id);
                CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Down, pos, id);
                break;
        }
    }
}