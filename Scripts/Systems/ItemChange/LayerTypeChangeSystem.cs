using System.Collections.Generic;
using Entitas;

public class LayerTypeChangeSystem : ReactiveSystem<GameEntity>
{
    public LayerTypeChangeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isLayerTypeChanging;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isLayerTypeChanging = false;
            entity.isCanBeActivatedByNearMatch = !entity.isCanBeActivatedByNearMatch;
        }
    }
}