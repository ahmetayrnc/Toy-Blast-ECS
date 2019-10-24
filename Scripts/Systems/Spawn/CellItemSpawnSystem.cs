using Entitas;

public class CellItemSpawnSystem : IExecuteSystem, IInitializeSystem
{
    readonly IGroup<GameEntity> _willSpawns;
    private readonly Contexts _contexts;

    public CellItemSpawnSystem(Contexts contexts)
    {
        _willSpawns = contexts.game.GetGroup(GameMatcher.WillSpawnCellItem);
        _contexts = contexts;
    }

    public void Initialize()
    {
        SpawnCellItems();
    }

    public void Execute()
    {
        SpawnCellItems();
    }

    private void SpawnCellItems()
    {
        if (_willSpawns.count <= 0)
        {
            return;
        }

        foreach (var e in _willSpawns.GetEntities())
        {
            e.willSpawnCellItem.Type.Spawn(e);
            e.RemoveWillSpawnCellItem();
        }

        _contexts.game.isCellsDirty = true;
        _contexts.game.isDirty = true;
    }
}