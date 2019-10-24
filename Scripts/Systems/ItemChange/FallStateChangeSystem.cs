using System.Collections.Generic;
using Entitas;

public class FallStateChangeSystem : ReactiveSystem<GameEntity>
{
    public FallStateChangeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isFallStateChanging;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isFallStateChanging = false;
            entity.isCanFall = !entity.isCanFall;
        }
    }
}