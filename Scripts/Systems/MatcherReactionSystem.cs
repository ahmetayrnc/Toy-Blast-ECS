using System.Collections.Generic;
using Entitas;

public class MatcherReactionSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public MatcherReactionSystem(Contexts context) : base(context.game)
    {
        _contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(
            GameMatcher.WillSpawnItem,
            GameMatcher.WillSpawnCellItem,
            GameMatcher.WillBeDestroyed,
            GameMatcher.Falling,
            GameMatcher.GridPosition));
    }

    protected override bool Filter(GameEntity entity)
    {
        return true; //entity.isWillBeDestroyed || entity.hasFalling || entity.hasGridPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.game.isDirty = true;
        Clear(); //to cleat collected entities just in case
    }
}