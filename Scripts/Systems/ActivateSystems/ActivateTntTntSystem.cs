using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ActivateTntTntSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateTntTntSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasTntTnt;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateTntTnt(entity);
        }
    }

    private void ActivateTntTnt(GameEntity tntTnt)
    {
        WaitHelper.Increase(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);

        var pos = tntTnt.gridPosition.value;
        var radius = tntTnt.tntTnt.Radius;

        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int y = pos.y - radius; y <= pos.y + radius; y++)
            {
                CellHelper.BlockFallAt(new Vector2Int(x, y));
            }
        }

        tntTnt.isSpawnAnimationStarted = true;
    }
}

public class TntTntExplosionEnderSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public TntTntExplosionEnderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.SpawnAnimationEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTntTnt && entity.isSpawnAnimationEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var tntTnt in entities)
        {
            tntTnt.isSpawnAnimationStarted = false;
            tntTnt.isSpawnAnimationEnded = false;
            
            var pos = tntTnt.gridPosition.value;
            var radius = tntTnt.tntTnt.Radius;
            var removerId = IdHelper.GetNewRemoverId();

            for (int x = pos.x - radius; x <= pos.x + radius; x++)
            {
                for (int y = pos.y - radius; y <= pos.y + radius; y++)
                {
                    CellHelper.UnBlockFallAt(new Vector2Int(x, y));

                    ActivatorHelper.TryActivateItemWithPositive(new Vector2Int(x, y), removerId,
                        ActivationReason.Tnt);
                }
            }

            tntTnt.isWillBeDestroyed = true;
        
            WaitHelper.Reduce(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);
        }
    }
}