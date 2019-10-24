using System.Collections;
using System.Collections.Generic;
using Entitas;
using EnumeratorExtensions;
using UnityEngine;

public class ActivatePuzzlePuzzleSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public ActivatePuzzlePuzzleSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.isPuzzlePuzzle;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivatePuzzlePuzzle(entity).Start();
        }
    }

    private IEnumerator ActivatePuzzlePuzzle(GameEntity entity)
    {
        WaitHelper.Increase(WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);

        yield return DoWait.WaitWhile(() => entity.isCreatedFromMatch);

        var width = _contexts.game.board.Size.x;
        var height = _contexts.game.board.Size.y;

        var pos = entity.gridPosition.value;
        var removerId = IdHelper.GetNewRemoverId();
        var radius = 0;
        var radiusLimit = (int) Mathf.Sqrt(width * width + height * height) + 1;

        var visited = new bool[width, height];
        while (radius < radiusLimit)
        {
            ProcessPuzzlePuzzle(pos, radius, removerId, visited);
            radius++;

            yield return DoWait.WaitSeconds(0.07f);
        }

        entity.isWillBeDestroyed = true;

        WaitHelper.Reduce(WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);
    }

    private void ProcessPuzzlePuzzle(Vector2Int center, int radius, int removerId, bool[,] visited)
    {
        var width = _contexts.game.board.Size.x;
        var height = _contexts.game.board.Size.y;
        var x = 0;
        var y = radius;
        var d = (int) -((uint) radius >> 1);

        while (x <= y)
        {
            if (center.x + x >= 0 && center.x + x <= width - 1 && center.y + y >= 0 && center.y + y <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x + x, center.y + y, removerId, visited);

            if (center.x + x >= 0 && center.x + x <= width - 1 && center.y - y >= 0 && center.y - y <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x + x, center.y - y, removerId, visited);

            if (center.x - x >= 0 && center.x - x <= width - 1 && center.y + y >= 0 && center.y + y <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x - x, center.y + y, removerId, visited);

            if (center.x - x >= 0 && center.x - x <= width - 1 && center.y - y >= 0 && center.y - y <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x - x, center.y - y, removerId, visited);

            if (center.x + y >= 0 && center.x + y <= width - 1 && center.y + x >= 0 && center.y + x <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x + y, center.y + x, removerId, visited);

            if (center.x + y >= 0 && center.x + y <= width - 1 && center.y - x >= 0 && center.y - x <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x + y, center.y - x, removerId, visited);

            if (center.x - y >= 0 && center.x - y <= width - 1 && center.y + x >= 0 && center.y + x <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x - y, center.y + x, removerId, visited);

            if (center.x - y >= 0 && center.x - y <= width - 1 && center.y - x >= 0 && center.y - x <= height - 1)
                ActivateBlockWithPuzzlePuzzle(center.x - y, center.y - x, removerId, visited);

            if (d <= 0)
            {
                x++;
                d += x;
            }
            else
            {
                y--;
                d -= y;
            }
        }
    }

    private void ActivateBlockWithPuzzlePuzzle(int x, int y, int removerId, bool[,] visited)
    {
        if (visited[x, y])
        {
            return;
        }

        visited[x, y] = true;

        ActivatorHelper.TryActivateItemWithPositive(new Vector2Int(x, y), removerId,
            ActivationReason.PuzzlePuzzle);
    }
}