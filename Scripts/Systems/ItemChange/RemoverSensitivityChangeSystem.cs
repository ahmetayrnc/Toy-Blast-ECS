using System.Collections.Generic;
using Entitas;

public class RemoverSensitivityChangeSystem : ReactiveSystem<GameEntity>
{
    public RemoverSensitivityChangeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isRemoverSensitivityChanging;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isRemoverSensitivityChanging = false;
            entity.isRemoverSensitive = !entity.isRemoverSensitive;
        }
    }
}