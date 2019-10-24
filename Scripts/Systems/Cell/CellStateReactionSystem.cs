using System.Collections.Generic;
using Entitas;

public class CellStateReactionSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public CellStateReactionSystem(Contexts context) : base(context.game)
    {
        _contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillBeDestroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.game.isCellsDirty = true;
        Clear(); //to cleat collected entities just in case
    }
}