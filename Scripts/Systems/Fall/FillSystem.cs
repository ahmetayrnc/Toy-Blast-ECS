using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class FillSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    private GameEntity[] _cells;
    private readonly List<Vector2> _fallingItems = new List<Vector2>();

    readonly IGroup<GameEntity> _cellGroup;
    readonly IGroup<GameEntity> _fallingItemGroup;

    private int _width;
    private int _height;

    public FillSystem(Contexts contexts)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Cell));
        _fallingItemGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Item, GameMatcher.Falling));
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cells = new GameEntity[_width * _height];
    }

    public void Execute()
    {
        var board = _contexts.game.board;
        var boardSize = board.Size;
        var height = boardSize.y;
        var width = boardSize.x;

        _fallingItems.Clear();

        foreach (var e in _cellGroup)
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _cells[key] = e;
        }

        foreach (var item in _fallingItemGroup)
        {
            if (item.position.value.y > height)
            {
                _fallingItems.Add(item.position.value);
            }
        }

        for (int x = 0; x < width; x++)
        {
            int firstBlockerY = FindFirstBlocker(x);

            for (int y = firstBlockerY + 1; y < height; y++)
            {
                var key = _width * y + x;

                if (_cells[key].cellState.Value == CellState.Invalid)
                {
                    continue;
                }

                float maxY = height + 1;
                for (int j = 0; j < _fallingItems.Count; j++)
                {
                    if ((int) _fallingItems[j].x == x)
                    {
                        if (_fallingItems[j].y > maxY)
                        {
                            maxY = _fallingItems[j].y;
                        }
                    }
                }

                var worldY = maxY + 1;

                _fallingItems.Add(new Vector2(x, worldY));

                CreateItemSpawn(x, worldY, y);
            }
        }
    }

    private int FindFirstBlocker(int x)
    {
        for (int y = _height - 1; y >= 0; y--)
        {
            var key = _width * y + x;

            if (_cells[key].cellState.Value == CellState.Invalid)
            {
                continue;
            }

            if (_cells[key].cellState.Value == CellState.Full)
            {
                return y;
            }

            if (!_cells[key].canAcceptFall.Value)
            {
                return y;
            }

            if (_cells[key].hasItemReservation)
            {
                return y;
            }
        }

        return -1;
    }

    private void CreateItemSpawn(int x, float worldY, int targetY)
    {
        var gridPos = new Vector2Int(x, targetY);
        var instance = _contexts.game.CreateEntity();

        //Todo add ratios
        var dropGroups = _contexts.game.itemGroups.Groups.Where(g => g != null && g.UseForDrop).ToList();
        var dropGroup = dropGroups[Random.Range(0, dropGroups.Count)];
        var dropItem = dropGroup.Items[Random.Range(0, dropGroup.Items.Count)];

        instance.AddWillSpawnItem(dropItem, gridPos, worldY);
        instance.AddId(IdHelper.GetNewId());
        instance.AddWillFill(0f);
    }
}