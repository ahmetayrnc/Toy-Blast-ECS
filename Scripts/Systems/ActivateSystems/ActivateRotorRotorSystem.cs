using System.Collections.Generic;
using Entitas;

public class ActivateRotorRotorSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateRotorRotorSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.isRotorRotor;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateRotorRotor(entity);
        }
    }

    private void ActivateRotorRotor(GameEntity entity)
    {
        WaitHelper.Increase(WaitType.CriticalAnimation);
        CellHelper.BlockFallAt(entity.gridPosition.value);

        entity.isSpawnAnimationStarted = true;
    }
}

public class RotorRotorSpawnEnderSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public RotorRotorSpawnEnderSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.SpawnAnimationEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isRotorRotor && entity.isSpawnAnimationEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.isSpawnAnimationStarted = false;
            entity.isSpawnAnimationEnded = false;
            CreateActiveRotors(entity);
            entity.isWillBeDestroyed = true;

            CellHelper.UnBlockFallAt(entity.gridPosition.value);
            WaitHelper.Reduce(WaitType.CriticalAnimation);
        }
    }

    private void CreateActiveRotors(GameEntity entity)
    {
        var removerId = IdHelper.GetNewRemoverId();
        var pos = entity.gridPosition.value;

        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Left, pos, removerId);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Right, pos, removerId);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Up, pos, removerId);
        CreateActivePositiveItemService.CreateActiveRotor(_contexts, RotorDirection.Down, pos, removerId);
    }
}