using System.Collections.Generic;
using Entitas;

public class SubItemSystem : ReactiveSystem<GameEntity>
{
    public SubItemSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillBeDestroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isSubItem;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            CreatePositiveItemService.CreateRandomPositiveItem(entity.gridPosition.value);
        }
    }
}