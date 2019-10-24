using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public sealed class BoardSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public BoardSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var levelNo = _contexts.game.level.LevelNo;

        var selectedLevel = LevelParser.ParseLevel(levelNo);
        SpawnLevel(selectedLevel);
    }

    private void SpawnLevel(Level level)
    {
        var width = level.Width;
        var height = level.Height;

        AddUniqueComponents(level);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var y = height - 1 - i;
                var x = j;
                CreateCell(level.ItemGrid[i, j], x, y);
                CreateItem(level.ItemGrid[i, j], x, y);
                CreateCellItem(level.CellItemGrid[i, j], x, y);
            }
        }
    }

    private void AddUniqueComponents(Level level)
    {
        var boardSize = new Vector2Int(level.Width, level.Height);

        _contexts.game.CreateEntity().AddGameplayState(GameplayState.Play);
        _contexts.game.CreateEntity().AddIdCounter(0);
        _contexts.game.CreateEntity().AddRemoverIdCount(0);
        _contexts.game.CreateEntity().AddReservationIdCounter(0);
        _contexts.game.CreateEntity().AddCriticalAnimationCounter(0);
        _contexts.game.CreateEntity().AddCollectAnimationCounter(0);
        _contexts.game.CreateEntity().AddBoard(boardSize, true, true, true, 0, 0, 0);
        _contexts.game.CreateEntity().AddTurn(0, 0, 0, 0);
        _contexts.game.CreateEntity().AddRemainingMoves(level.MoveCount);
        _contexts.game.CreateEntity().AddItemGroups(level.Groups);

        var progress = new Dictionary<GoalType, int>(level.Goal);
        var goalForGenerators = new Dictionary<GoalType, int>(level.Goal);

        foreach (var entry in progress.ToList())
        {
            progress[entry.Key] = 0;
        }

        _contexts.game.CreateEntity().AddGoal(level.Goal);
        _contexts.game.CreateEntity().AddGoalProgress(progress);
        _contexts.game.CreateEntity().AddGoalForGenerators(goalForGenerators);
    }

    private void CreateItem(ItemTypeInBoard type, int x, int y)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddWillSpawnItem(type, new Vector2Int(x, y), y);
        entity.AddId(IdHelper.GetNewId());
    }

    private void CreateCell(ItemTypeInBoard type, int x, int y)
    {
        var entity = _contexts.game.CreateEntity();

        var pos = new Vector2(x, y);
        entity.AddPosition(pos);
        entity.AddGridPosition(new Vector2Int(x, y));
        entity.AddCellId(new Tuple<int, int>(x, y));
        entity.AddCellState(Equals(type, ItemTypeInBoard.Invalid) ? CellState.Invalid : CellState.Empty);
        entity.AddCanAcceptFall(true, 0);
        entity.AddId(IdHelper.GetNewId());

        entity.AddCell(true, true);
    }

    private void CreateCellItem(CellItemTypeInBoard type, int x, int y)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddWillSpawnCellItem(type, x, y);
    }
}