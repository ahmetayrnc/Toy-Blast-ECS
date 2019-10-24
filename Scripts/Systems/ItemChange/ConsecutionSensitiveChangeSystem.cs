using System.Collections.Generic;
using Entitas;

public class ConsecutionSensitiveChangeSystem : ReactiveSystem<GameEntity>
{
    public ConsecutionSensitiveChangeSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasConsecutionSensitive;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.ReplaceConsecutionSensitive(Contexts.sharedInstance.game.turn.TurnId);
        }
    }
}