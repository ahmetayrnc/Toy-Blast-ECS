using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CandyExplosionEnderSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public CandyExplosionEnderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ExplosionEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasExplosionEnded && entity.hasOrderedFakeItems;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ExplodeCandy(entity);
            ReduceCandyMonsterLayer(entity);
            entity.RemoveExplosionEnded();
//            WaitHelper.Reduce(WaitType.CriticalAnimation);
        }
    }

    private void ReduceCandyMonsterLayer(GameEntity entity)
    {
        var layerValue = entity.layer.Value;
        entity.ReplaceLayer(Mathf.Max(0, layerValue - 1));

        if (layerValue > 0) return;

        entity.isWillBeDestroyed = true;
    }

    private void ExplodeCandy(GameEntity entity)
    {
        var fakeItemId = entity.orderedFakeItems.FakeItemIds.Pop();
        var fakeItem = _contexts.game.GetEntityWithId(fakeItemId);
        fakeItem.isWillBeDestroyed = true;
    }
}

public class CandyExplosionStarterSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public CandyExplosionStarterSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillExplode);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasWillExplode && entity.hasOrderedFakeItems;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.AddExplosionStarted(entity.willExplode.Count);
//            WaitHelper.Increase(WaitType.CriticalAnimation);
        }
    }
}