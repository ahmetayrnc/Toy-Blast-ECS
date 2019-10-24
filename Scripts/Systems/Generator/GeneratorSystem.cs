using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using ItemExtensions;
using UnityEngine;
using Random = System.Random;

public class GeneratorSystem : ReactiveSystem<GameEntity>, ICleanupSystem, IInitializeSystem
{
    private readonly Contexts _contexts;
    private int _width;
    private int _height;

    public GeneratorSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WillGenerate);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isWillGenerate;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.isGeneratorClosed) continue;

            var generationAmount = entity.generationAmount.Value;
            var generatorType = entity.generator.Type;
            var bottomCellConsiderate = entity.isBottomCellConsiderate;
            var radius = entity.generatorRadius.Value;

            switch (generatorType)
            {
                case GeneratorType.Item:
                    GenerateItem(entity, generationAmount, entity.itemToGenerate.Type, bottomCellConsiderate, radius);
                    break;
                case GeneratorType.CellItem:
                    GenerateCellItem(entity, generationAmount, entity.cellItemToGenerate.Type, radius);
                    break;
            }
        }
    }

    private void GenerateCellItem(GameEntity generator, int defaultAmount, CellItemTypeInBoard type,
        GeneratorRadius generatorRadius)
    {
        var generationAmount = generator.goalEffectType.Type == GoalEffectType.Decrease
            ? CalculateGenerationAmountForCellItem(type, defaultAmount)
            : defaultAmount;

        var positionsToCheck = GetPositionsToCheck(generator.gridPosition.value, generatorRadius);

        var cellItemIds = SelectSuitableCellItems(positionsToCheck).Shuffle().Take(generationAmount).ToList();

        var reservations = new List<int>();
        foreach (var id in cellItemIds)
        {
            var reservationId = IdHelper.GetNewReservationId();
            var cell = _contexts.game.GetEntityWithCellId(id);
            cell.AddCellItemReservation(reservationId, type);
            reservations.Add(reservationId);
            WaitHelper.Increase(WaitType.CriticalAnimation);
        }

        generator.AddReservedCells(reservations);

        if (generator.goalEffectType.Type == GoalEffectType.Decrease)
        {
            _contexts.game.goalForGenerators.Goals[type.GoalType] -= generationAmount;

            if (CalculateCellItemLeftToGenerate(type) == 0)
            {
                CloseGenerators(generator);
            }
        }
        else
        {
            GoalHelper.AddToGoal(type.GoalType, reservations.Count);
        }
    }

    private void GenerateItem(GameEntity generator, int amount, ItemTypeInBoard type, bool bottomCellConsiderate,
        GeneratorRadius generatorRadius)
    {
        var generationAmount = generator.goalEffectType.Type == GoalEffectType.Decrease
            ? CalculateGenerationAmountForItem(type, amount)
            : amount;

        var positionsToCheck = GetPositionsToCheck(generator.gridPosition.value, generatorRadius);

        var itemIds = SelectSuitableItems(positionsToCheck, bottomCellConsiderate).Shuffle().Take(generationAmount)
            .ToList();

        var reservations = new List<Tuple<int, int>>();
        foreach (var id in itemIds)
        {
            var reservationId = IdHelper.GetNewReservationId();
            var item = _contexts.game.GetEntityWithId(id);
            item.AddItemReservation(reservationId, id, type);
            reservations.Add(new Tuple<int, int>(reservationId, id));
            WaitHelper.Increase(WaitType.CriticalAnimation);
        }

        generator.AddReservedItems(reservations);

        if (generator.goalEffectType.Type == GoalEffectType.Decrease)
        {
            _contexts.game.goalForGenerators.Goals[type.GoalType] -= generationAmount;

            if (CalculateItemLeftToGenerate(type) == 0)
            {
                CloseGenerators(generator);
            }
        }
    }


    private int CalculateGenerationAmountForItem(ItemTypeInBoard type, int defaultAmount)
    {
        var itemCountInBoard = _contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.GoalType))
            .Count(e => Equals(e.goalType.Value, type.GoalType));
        var itemCountInReservations = _contexts.game.GetEntities(GameMatcher.ItemReservation)
            .Count(e => Equals(e.itemReservation.ItemType, type));
        var itemCountLeftInGoal = CalculateItemCountLeftInGoal(type.GoalType);

        var generationAmount =
            Mathf.Min(defaultAmount, itemCountLeftInGoal - (itemCountInBoard + itemCountInReservations));
        return Mathf.Max(generationAmount, 0);
    }

    private int CalculateGenerationAmountForCellItem(CellItemTypeInBoard type, int defaultAmount)
    {
        var cellItemCountInBoard = _contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.GoalType))
            .Count(e => Equals(e.goalType.Value, type.GoalType));
        var cellItemReservations = _contexts.game.GetEntities(GameMatcher.CellItemReservation)
            .Count(e => Equals(e.cellItemReservation.CellItemType, type));
        var itemCountLeftInGoal = CalculateItemCountLeftInGoal(type.GoalType);

        var generationAmount =
            Mathf.Min(defaultAmount, itemCountLeftInGoal - (cellItemCountInBoard + cellItemReservations));
        return Mathf.Max(generationAmount, 0);
    }

    private int CalculateItemCountLeftInGoal(GoalType type)
    {
        return _contexts.game.goal.Goals[type] - _contexts.game.goalProgress.Progress[type];
    }

    private int CalculateItemLeftToGenerate(ItemTypeInBoard type)
    {
        var itemCountInBoard = _contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.Item))
            .Count(e => Equals(e.itemType.Value, type.ItemType));
        var itemCountInReservations = _contexts.game.GetEntities(GameMatcher.ItemReservation)
            .Count(e => Equals(e.itemReservation.ItemType, type));
        var itemCountLeftInGoal = CalculateItemCountLeftInGoal(type.GoalType);

        return itemCountLeftInGoal - (itemCountInBoard + itemCountInReservations);
    }

    private int CalculateCellItemLeftToGenerate(CellItemTypeInBoard type)
    {
        var cellItemCountInBoard = _contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.CellItem))
            .Count(e => Equals(e.cellItemType.Value, type.CellItemType));
        var cellItemReservations = _contexts.game.GetEntities(GameMatcher.CellItemReservation)
            .Count(e => Equals(e.cellItemReservation.CellItemType, type));
        var cellItemCountLeftInGoal = CalculateItemCountLeftInGoal(type.GoalType);

        return cellItemCountLeftInGoal - (cellItemCountInBoard + cellItemReservations);
    }

    private void CloseGenerators(GameEntity generator)
    {
        foreach (var g in _contexts.game.GetEntities(GameMatcher.Generator)
            .Where(e => Equals(e.itemType.Value, generator.itemType.Value)))
        {
            g.isGeneratorClosed = true;
            g.isCantBeActivated = true;
        }
    }

    private List<int> SelectSuitableItems(List<Tuple<int, int>> positionsToCheck, bool bottomCellConsiderate)
    {
        var itemIds = new List<int>();
        foreach (var pos in positionsToCheck)
        {
            var cell = _contexts.game.GetEntityWithCellId(pos);
            if (cell == null || cell.cellState.Value == CellState.Invalid) continue;
            if (!cell.cell.CanLetItemActivate) continue;
            if (bottomCellConsiderate && (!cell.isHasAccessToBottom || cell.isBottomCell)) continue;

            var item = _contexts.game.GetItemWithPosition(pos);
            if (item == null) continue;
            if (item.hasItemReservation) continue;
            if (!item.isCanBeTargetedByGenerator) continue;

            itemIds.Add(item.id.Value);
        }

        return itemIds;
    }

    private List<Tuple<int, int>> SelectSuitableCellItems(List<Tuple<int, int>> positionsToCheck)
    {
        var cellItemIds = new List<Tuple<int, int>>();
        foreach (var pos in positionsToCheck)
        {
            var cell = _contexts.game.GetEntityWithCellId(pos);
            if (cell == null || cell.cellState.Value == CellState.Invalid) continue;
            if (cell.hasCellItemReservation) continue;

            var cellItem = _contexts.game.GetEntityWithCellItemId(pos);
            if (cellItem != null) continue;

            var item = _contexts.game.GetItemWithPosition(pos);
            if (item != null && item.isCantHaveBubbleOnTop) continue;

            cellItemIds.Add(pos);
        }

        return cellItemIds;
    }

    private List<Tuple<int, int>> GetPositionsToCheck(Vector2Int generatorPos, GeneratorRadius generatorRadius)
    {
        var positions = new List<Tuple<int, int>>();
        switch (generatorRadius)
        {
            case GeneratorRadius.All:
                for (int x = 0; x < _contexts.game.board.Size.x; x++)
                {
                    for (int y = 0; y < _contexts.game.board.Size.y; y++)
                    {
                        positions.Add(new Tuple<int, int>(x, y));
                    }
                }

                break;

            case GeneratorRadius.Radius1:
                for (int x = generatorPos.x - 1; x <= generatorPos.x + 1; x++)
                {
                    for (int y = generatorPos.y - 1; y <= generatorPos.y + 1; y++)
                    {
                        if (!InBounds(x, y)) continue;
                        positions.Add(new Tuple<int, int>(x, y));
                    }
                }

                break;
        }

        return positions;
    }

    public void Cleanup()
    {
        foreach (var entity in _contexts.game.GetGroup(GameMatcher.WillGenerate).GetEntities())
        {
            entity.isWillGenerate = false;
        }
    }

    private bool InBounds(int x, int y)
    {
        return !(x < 0 || x >= _width || y < 0 || y >= _height);
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.Shuffle(new Random());
    }

    private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (rng == null) throw new ArgumentNullException(nameof(rng));

        return source.ShuffleIterator(rng);
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, Random rng)
    {
        List<T> buffer = source.ToList();
        for (int i = 0; i < buffer.Count; i++)
        {
            int j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
}