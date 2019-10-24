using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ActivateTntRotorSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateTntRotorSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.isTntRotor;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateTntRotor(entity);
        }
    }

    private void ActivateTntRotor(GameEntity entity)
    {
        WaitHelper.Increase(WaitType.CriticalAnimation);
        CellHelper.BlockFallAt(entity.gridPosition.value);
        entity.isSpawnAnimationStarted = true;
    }
}

public class TntRotorSpawnEnderSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public TntRotorSpawnEnderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.SpawnAnimationEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isTntRotor && entity.isSpawnAnimationEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isSpawnAnimationStarted = false;
            entity.isSpawnAnimationEnded = false;

            entity.isWillBeDestroyed = true;
            CreateActivateRotors(entity);
            CellHelper.UnBlockFallAt(entity.gridPosition.value);
            WaitHelper.Reduce(WaitType.CriticalAnimation);
        }
    }

    private void CreateActivateRotors(GameEntity entity)
    {
        var id = IdHelper.GetNewRemoverId();
        var pos = entity.gridPosition.value;

        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Up, new Vector2Int(pos.x - 1, pos.y), id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Up, pos, id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Up, new Vector2Int(pos.x + 1, pos.y), id);

        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Down, new Vector2Int(pos.x - 1, pos.y), id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Down, pos, id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Down, new Vector2Int(pos.x + 1, pos.y), id);

        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Left, new Vector2Int(pos.x, pos.y + 1), id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Left, pos, id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Left, new Vector2Int(pos.x, pos.y - 1), id);

        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Right, new Vector2Int(pos.x, pos.y + 1), id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Right, pos, id);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Right, new Vector2Int(pos.x, pos.y - 1), id);
    }
}