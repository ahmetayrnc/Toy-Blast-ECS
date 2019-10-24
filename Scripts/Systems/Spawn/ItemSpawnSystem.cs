using System;
using System.Collections.Generic;
using Entitas;

public class ItemSpawnSystem : IExecuteSystem, IInitializeSystem
{
    readonly IGroup<GameEntity> _willSpawns;
    private readonly Contexts _contexts;

    private Dictionary<ItemTypeInBoard, Action<GameEntity>> _spawnMethods;

    public ItemSpawnSystem(Contexts contexts)
    {
        _willSpawns = contexts.game.GetGroup(GameMatcher.WillSpawnItem);
        _contexts = contexts;
    }

    public void Initialize()
    {
        SpawnItems();
    }

    public void Execute()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        if (_willSpawns.count <= 0)
        {
            return;
        }

        foreach (var e in _willSpawns.GetEntities())
        {
            e.willSpawnItem.ItemType.Spawn(e);
            e.RemoveWillSpawnItem();
        }

        _contexts.game.isCellsDirty = true;
        _contexts.game.isDirty = true;
    }
}