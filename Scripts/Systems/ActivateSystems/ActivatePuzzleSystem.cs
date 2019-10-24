using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using EntitasBlast;
using EnumeratorExtensions;

public class ActivatePuzzleSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _itemGroup;

    public ActivatePuzzleSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _itemGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Item)
            .NoneOf(GameMatcher.Activation, GameMatcher.WillBeDestroyed, GameMatcher.CantBeActivated));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasItemType && Equals(entity.itemType.Value, ItemType.Puzzle);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivatePuzzle(entity).Start();
        }
    }

    private IEnumerator ActivatePuzzle(GameEntity puzzle)
    {
        yield return DoWait.WaitUntil(() => !WaitHelper.Has(WaitType.FallingItem));

        WaitHelper.Increase(WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);

        TryDestroyBubbleOnTop(puzzle);

        var cubes = GetColorCubes(puzzle.color.Value);

        puzzle.AddPuzzleTargetedCubes(cubes);

        yield return DoWait.WaitWhile(() => puzzle.hasPuzzleTargetedCubes);

        ActivateColorCubes(cubes);

        puzzle.isWillBeDestroyed = true;

        WaitHelper.Reduce(WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);
    }

    private void TryDestroyBubbleOnTop(GameEntity puzzle)
    {
        var bubbles = _contexts.game.GetEntitiesWithGridPosition(puzzle.gridPosition.value)
            .Where(e => e.isCellItem && e.isCanBeActivatedByInnerMatch).ToList();

        if (bubbles.Count == 0)
        {
            return;
        }

        var bubble = bubbles[0];

        bubble.isWillBeDestroyed = true;
    }

    private List<int> GetColorCubes(Color color)
    {
        var cubes = new List<int>();

        foreach (var item in _itemGroup.GetEntities())
        {
            var cellItem = _contexts.game.GetEntityWithCellItemId(
                new Tuple<int, int>(item.gridPosition.value.x, item.gridPosition.value.y));

            if (!Equals(item.itemType.Value, ItemType.ColorCube))
            {
                continue;
            }

            if (cellItem != null && cellItem.isCanStopItemActivation)
            {
                continue;
            }

            var itemColor = item.color.Value;

            if (color != itemColor)
            {
                continue;
            }

            cubes.Add(item.id.Value);
        }

        return cubes;
    }

    private void ActivateColorCubes(List<int> ids)
    {
        foreach (var id in ids)
        {
            var item = _contexts.game.GetEntityWithId(id);
            if (item == null)
                continue;

            if (item.hasRemoverId)
                continue;

            var cellItem = _contexts.game.GetEntityWithCellItemId(
                new Tuple<int, int>(item.gridPosition.value.x, item.gridPosition.value.y));
            if (cellItem != null && cellItem.isCanBeActivatedByInnerMatch)
            {
                cellItem.isWillBeDestroyed = true;
            }

            ActivatorHelper.ActivateItem(item, ActivationReason.Puzzle);
            var removerId = IdHelper.GetNewRemoverId();
            item.AddRemoverId(removerId);
        }
    }
}