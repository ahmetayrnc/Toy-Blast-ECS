using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using EnumeratorExtensions;
using UnityEngine;
using Color = EntitasBlast.Color;

public class ActivatePuzzleComboSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _itemGroup;

    public ActivatePuzzleComboSystem(Contexts contexts) : base(contexts.game)
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
        return entity.isActivated && entity.hasPuzzleCombo;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            ActivatePuzzleCombo(entity).Start();
        }
    }

    private IEnumerator ActivatePuzzleCombo(GameEntity puzzleCombo)
    {
        CellHelper.BlockFallAt(puzzleCombo.gridPosition.value);

        yield return null;

        yield return new WaitWhile(() => WaitHelper.Has(WaitType.FallingItem));

        WaitHelper.Increase(WaitType.Hint, WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);

        var cubes = GetColorCubes(puzzleCombo.color.Value);

        puzzleCombo.AddPuzzleTargetedCubes(cubes);

        yield return new WaitWhile(() => puzzleCombo.hasPuzzleTargetedCubes);

        yield return new WaitUntil(() => puzzleCombo.hasPosItemsToActivate);

        CellHelper.UnBlockFallAt(puzzleCombo.gridPosition.value);

        puzzleCombo.isWillBeDestroyed = true;

        yield return ActivatePositiveItemsSequentially(puzzleCombo.posItemsToActivate.PosItemIds);

        WaitHelper.Reduce(WaitType.Hint, WaitType.Input, WaitType.Fall, WaitType.Turn, WaitType.CriticalAnimation);
    }

    private List<int> GetColorCubes(Color color)
    {
        var cubes = new List<int>();

        foreach (var item in _itemGroup.GetEntities())
        {
            var entitySet = _contexts.game.GetEntitiesWithGridPosition(item.gridPosition.value);
            GameEntity cellItem = null;
            foreach (var entity in entitySet)
            {
                if (entity.isCellItem)
                {
                    cellItem = entity;
                }
            }

            if (!Equals(item.itemType.Value, ItemType.ColorCube))
            {
                continue;
            }

            if (cellItem != null && cellItem.isCanStopItemActivation)
            {
                continue;
            }

            Color itemColor = item.color.Value;

            if (color != itemColor)
            {
                continue;
            }

            cubes.Add(item.id.Value);
        }

        return cubes;
    }

    private IEnumerator ActivatePositiveItemsSequentially(List<int> ids)
    {
        //activate positive items sequentially
        foreach (var id in ids)
        {
            if (Contexts.sharedInstance.game.GetEntityWithId(id) == null)
            {
                continue;
            }

            yield return DoWait.WaitSeconds(0.08f);

            var posItem = Contexts.sharedInstance.game.GetEntityWithId(id);
            if (posItem == null)
            {
                continue;
            }

            var cellItem = _contexts.game.GetEntityWithCellItemId(
                new Tuple<int, int>(posItem.gridPosition.value.x, posItem.gridPosition.value.y));
            if (cellItem != null && cellItem.isCanBeActivatedByInnerMatch)
            {
                cellItem.isWillBeDestroyed = true;
            }

            ActivatorHelper.ActivateItem(posItem, ActivationReason.Tnt);
        }
    }
}