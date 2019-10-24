using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CollectionAtEndSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    private readonly Contexts _contexts;

    public CollectionAtEndSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.WillExplode, GameMatcher.WillBeDestroyed));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollectedAtEnd && entity.isWillBeDestroyed && (entity.isItem || entity.isCellItem);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!GoalHelper.IsInGoal(entity.goalType.Value)) continue;

            entity.AddCollectionStarted(1);
            WaitHelper.Increase(WaitType.CollectAnimation);
        }
    }

    public void Cleanup()
    {
        foreach (var entity in _contexts.game.GetGroup(GameMatcher.CollectionStarted).GetEntities())
        {
            entity.RemoveCollectionStarted();
        }
    }
}

public class CollectionAtLayerSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public CollectionAtLayerSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.WillExplode));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCollectedAtLayer && entity.hasWillExplode && (entity.isItem || entity.isCellItem);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!GoalHelper.IsInGoal(entity.goalType.Value)) continue;

            var layerCount = entity.layer.Value + 1;
            var explodeAmount = entity.willExplode.Count;
            var collectAmount = Mathf.Min(layerCount, explodeAmount);
            entity.AddCollectionStarted(collectAmount);
            WaitHelper.Increase(WaitType.CollectAnimation);
        }
    }
}

public class CollectionEnderSystem : ReactiveSystem<GameEntity>
{
    public CollectionEnderSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.CollectionEnded));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollectionEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            GoalHelper.AddToProgress(entity.collectionEnded.GoalType, entity.collectionEnded.Amount);
            entity.isWillBeDestroyed = true;
            WaitHelper.Reduce(WaitType.CollectAnimation);
        }
    }
}