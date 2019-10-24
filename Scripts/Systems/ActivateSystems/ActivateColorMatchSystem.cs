using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using EnumeratorExtensions;
using UnityEngine;

public class ActivateColorMatchSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public ActivateColorMatchSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ColorMatch);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasColorMatch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivateColorMatch(entity);
            entity.isWillBeDestroyed = true;
        }
    }

    private void ActivateColorMatch(GameEntity colorMatch)
    {
        var touchedCube = _contexts.game.GetEntityWithId(colorMatch.colorMatch.TouchedCubeId);
        var matchId = touchedCube.matchGroup.Id;
        var matchCount = touchedCube.matchGroup.Count;
        var cubes = _contexts.game.GetEntitiesWithMatchGroup(matchId);
        var removerId = IdHelper.GetNewRemoverId();

        if (matchCount < 5)
        {
            WaitHelper.Increase(WaitType.Turn);

            foreach (var cube in cubes)
            {
                ActivateColorCube(cube, removerId);
                cube.AddWillExplode(1);
            }

            WaitHelper.Reduce(WaitType.Turn);
        }
        else
        {
            //this stops event async
            ActivateSpecialColorMatch(touchedCube, cubes, removerId).Start();
        }
    }

    private IEnumerator ActivateSpecialColorMatch(GameEntity touchedCube, IEnumerable<GameEntity> cubes, int removerId)
    {
        WaitHelper.Increase(WaitType.Turn, WaitType.Input, WaitType.CriticalAnimation);

        touchedCube.isMergeMaster = true;

        var cubeIds = new List<int>();
        foreach (var cube in cubes)
        {
            cubeIds.Add(cube.id.Value);

            ActivateColorCube(cube, removerId);

            cube.AddMerging(touchedCube.gridPosition.value);
            cube.isCanFall = false;
            cube.isCantBeActivated = true;
        }

        var touchedCubeId = touchedCube.id.Value;

        yield return DoWait.WaitWhile(() => touchedCube.isMergeMaster);

        CreatePositiveItem(touchedCubeId);

        foreach (var id in cubeIds)
        {
            var cube = _contexts.game.GetEntityWithId(id);

            if (cube == null)
            {
                continue;
            }

            cube.isWillBeDestroyed = true;
        }

        WaitHelper.Reduce(WaitType.Input, WaitType.Turn, WaitType.CriticalAnimation);
    }

    private void ActivateColorCube(GameEntity cube, int removerId)
    {
        var color = cube.color.Value;
        var cubePos = cube.gridPosition.value;

        var cellItem = _contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(cubePos.x, cubePos.y));

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
    }

    private bool InBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= _contexts.game.board.Size.x)
            return false;

        if (pos.y < 0 || pos.y >= _contexts.game.board.Size.y)
            return false;

        return true;
    }

    private void CreatePositiveItem(int touchedCubeId)
    {
        var touchedCube = _contexts.game.GetEntityWithId(touchedCubeId);

        Debug.Assert(touchedCube != null);

        var matchCount = touchedCube.matchGroup.Count;
        var pos = touchedCube.gridPosition.value;
        var color = touchedCube.color.Value;
        var posItemType = CreatePositiveItemService.PositiveItem.Rotor;

        if (matchCount >= 9)
        {
            posItemType = CreatePositiveItemService.PositiveItem.Puzzle;
        }
        else if (matchCount >= 7)
        {
            posItemType = CreatePositiveItemService.PositiveItem.Tnt;
        }
        else if (matchCount >= 5)
        {
            posItemType = CreatePositiveItemService.PositiveItem.Rotor;
        }

        var id = CreatePositiveItemService.CreatePositiveItem(posItemType, pos, color);
        var posItem = _contexts.game.GetEntityWithId(id);
        posItem.isCreatedFromMatch = true;
    }
}