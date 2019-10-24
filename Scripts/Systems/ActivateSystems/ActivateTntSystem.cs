using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ActivateTntSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateTntSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasItemType && entity.itemType.Value == ItemType.Tnt;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateTnt(entity);
        }
    }

    private void ActivateTnt(GameEntity tnt)
    {
        WaitHelper.Increase(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);

        tnt.isTntExplosionStarted = true;
        tnt.isCanFall = false;

        var tntPos = tnt.gridPosition.value;
        const int radius = 1;

        for (var x = tntPos.x - radius; x <= tntPos.x + radius; x++)
        {
            for (var y = tntPos.y - radius; y <= tntPos.y + radius; y++)
            {
                CellHelper.BlockFallAt(new Vector2Int(x, y));
            }
        }
    }
}

public class TntExplosionEnderSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public TntExplosionEnderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TntExplosionEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasItemType && Equals(entity.itemType.Value, ItemType.Tnt) && entity.isTntExplosionEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var tnt in entities)
        {
            var tntPos = tnt.gridPosition.value;
            const int radius = 1;
            var removerId = IdHelper.GetNewRemoverId();

            tnt.isTntExplosionEnded = false;
            tnt.isTntExplosionStarted = false;

            for (var x = tntPos.x - radius; x <= tntPos.x + radius; x++)
            {
                for (var y = tntPos.y - radius; y <= tntPos.y + radius; y++)
                {
                    CellHelper.UnBlockFallAt(new Vector2Int(x, y));

                    ActivatorHelper.TryActivateItemWithPositive(new Vector2Int(x, y), removerId,
                        ActivationReason.Tnt);
                }
            }

            tnt.isWillBeDestroyed = true;

            WaitHelper.Reduce(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);
        }
    }
}