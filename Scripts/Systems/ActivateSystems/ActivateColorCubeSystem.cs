using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ActivateColorCubeSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateColorCubeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasItemType && entity.itemType.Value == ItemType.ColorCube;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateColorCube(entity);
        }
    }

    private void ActivateColorCube(GameEntity cube)
    {
        var color = cube.color.Value;
        var cubePos = cube.gridPosition.value;
        var removerId = cube.removerId.Value;
        var entitySet = _contexts.game.GetEntitiesWithGridPosition(cubePos);

        GameEntity cellItem = null;
        foreach (var entity in entitySet)
        {
            if (entity.isCellItem)
            {
                cellItem = entity;
            }
        }

        if (cellItem != null && cellItem.isCanBeActivatedByInnerMatch)
        {
            cellItem.isWillBeDestroyed = true;
        }

        int x = cubePos.x;
        int y = cubePos.y;

        var positions = new[]
        {
            new Vector2Int(x - 1, y),
            new Vector2Int(x + 1, y),
            new Vector2Int(x, y - 1),
            new Vector2Int(x, y + 1),
        };

        for (int i = 0; i < positions.Length; i++)
        {
            var pos = positions[i];
            if (!InBounds(pos))
            {
                continue;
            }

            ActivatorHelper.TryActivateItemWithNear(_contexts, pos, removerId, color);
        }

        cube.isWillBeDestroyed = true;
    }

    private bool InBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= _contexts.game.board.Size.x)
            return false;

        if (pos.y < 0 || pos.y >= _contexts.game.board.Size.y)
            return false;

        return true;
    }
}