using System.Collections.Generic;
using System.Linq;
using Entitas;

public class HiveSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly Contexts _contexts;

    public HiveSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.WillExplode));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasWillExplode && entity.isItem && entity.isGoalDependent;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var goalType = entity.goalType.Value;
            if (_contexts.game.goalForGenerators.Goals[goalType] <= 0) continue;

            _contexts.game.goalForGenerators.Goals[goalType] -= entity.willExplode.Count;

            if (_contexts.game.goalForGenerators.Goals[goalType] > 0) continue;

            RemoveAllBeehives();
        }
    }

    private void RemoveAllBeehives()
    {
        foreach (var beehive in _contexts.game.GetEntities()
            .Where(e => e.isItem && Equals(e.itemType.Value, ItemType.Beehive)))
        {
            beehive.isWillBeDestroyed = true;
        }
    }

    public void Cleanup()
    {
    }
}