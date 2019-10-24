using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FallDetermination : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;

    private GameEntity[] _items;

    private GameEntity[] _cellEntities;

    readonly IGroup<GameEntity> _cellGroup;
    readonly IGroup<GameEntity> _itemGroup;

    private int _width;
    private int _height;

    public FallDetermination(Contexts contexts)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Cell));
        _itemGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Item, GameMatcher.CanFall)
            .NoneOf(GameMatcher.Activation, GameMatcher.WillBeDestroyed));
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cellEntities = new GameEntity[_width * _height];
        _items = new GameEntity[_width * _height];
    }

    public void Execute()
    {
        var board = _contexts.game.board;
        var boardSize = board.Size;
        var height = boardSize.y;
        var width = boardSize.x;
        var isFallActive = board.IsFallActive;

        if (!isFallActive)
        {
            return;
        }

        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var key = _width * y + x;
                _items[key] = null;
            }
        }

        foreach (var item in _itemGroup)
        {
            //bounds check
            var pos = item.gridPosition.value;

            if (pos.y >= height)
            {
                continue;
            }

            var key = _width * pos.y + pos.x;
            _items[key] = item;
        }

        foreach (var cell in _cellGroup)
        {
            var pos = cell.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _cellEntities[key] = cell;
        }

        for (int y = 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector2Int(x, y);
                var key = _width * y + x;

                if (_items[key] == null)
                {
                    continue;
                }

                if (_items[key].isMultiBlock)
                {
                    MultiFall(_items[key], pos);
                }
                else
                {
                    SingleFall(_items[key], pos);
                }
            }
        }
    }

    private void SingleFall(GameEntity item, Vector2Int pos)
    {
        var curPos = pos;

        var targetPos = FindTargetCell(curPos);

        if (!CanFall(curPos, targetPos))
        {
            return;
        }

        if (!item.hasFalling)
        {
            item.AddFalling((Vector2) curPos, (Vector2) targetPos, 0f);
        }
        else
        {
            item.ReplaceFalling((Vector2) curPos, (Vector2) targetPos, item.falling.Speed);
        }

        ItemLeftCell(_cellEntities[EnKey(curPos)]);
        ItemEnteredCell(_cellEntities[EnKey(targetPos)]);
        item.ReplaceGridPosition(targetPos);
    }

    private Vector2Int FindTargetCell(Vector2Int pos)
    {
        Vector2Int targetPos = new Vector2Int(pos.x, pos.y - 1);

        while (_cellEntities[EnKey(targetPos)].cellState.Value == CellState.Invalid)
        {
            targetPos = new Vector2Int(targetPos.x, targetPos.y - 1);

            if (targetPos.y < 0)
            {
                return targetPos;
            }
        }

        return targetPos;
    }

    //Assumes 2 by 2
    private void MultiFall(GameEntity item, Vector2Int pos)
    {
        var left = pos;
        var leftTarget = FindTargetCellToStand(left);
        leftTarget.y += 1;

        var right = new Vector2Int(pos.x + 1, pos.y);
        var rightTarget = FindTargetCellToStand(right);
        rightTarget.y += 1;

        if (rightTarget.y > leftTarget.y)
        {
            leftTarget.y = rightTarget.y;
        }

        if (leftTarget == left) return;

        var bottomLeft = new Vector2Int(leftTarget.x, leftTarget.y);
        var bottomRight = new Vector2Int(leftTarget.x + 1, leftTarget.y);
        var topLeft = new Vector2Int(leftTarget.x, leftTarget.y + 1);
        var topRight = new Vector2Int(leftTarget.x + 1, leftTarget.y + 1);

        var oldPositions = new List<Vector2Int>
        {
            new Vector2Int(left.x, left.y),
            new Vector2Int(left.x + 1, left.y),
            new Vector2Int(left.x, left.y + 1),
            new Vector2Int(left.x + 1, left.y + 1),
        };

        if
        (
            !IsSuitableMulti(bottomLeft, oldPositions) ||
            !IsSuitableMulti(bottomRight, oldPositions) ||
            !IsSuitableMulti(topLeft, oldPositions) ||
            !IsSuitableMulti(topRight, oldPositions)
        )
        {
            return;
        }

        if (!item.hasFalling)
        {
            item.AddFalling((Vector2) left, (Vector2) leftTarget, 0f);
        }
        else
        {
            item.ReplaceFalling((Vector2) left, (Vector2) leftTarget, item.falling.Speed);
        }

        var fallHeight = left.y - bottomLeft.y;
        var fakeItemIds = item.fakeItems.FakeItemIds;
        GameEntity[] fakeItems = new GameEntity[3];

        ItemLeftCell(_cellEntities[EnKey(left)]);
        ItemEnteredCell(_cellEntities[EnKey(leftTarget)]);
        item.ReplaceGridPosition(leftTarget);

        for (int i = 0; i < fakeItemIds.Count; i++)
        {
            var fakeItem = _contexts.game.GetEntityWithId(fakeItemIds[i]);
            var fakeItemPos = fakeItem.gridPosition.value;
            ItemLeftCell(_cellEntities[EnKey(fakeItemPos)]);
            fakeItems[i] = fakeItem;
        }

        for (int i = 0; i < fakeItems.Length; i++)
        {
            var fakeItem = fakeItems[i];
            var newPos = fakeItem.gridPosition.value;
            newPos.y -= fallHeight;
            ItemEnteredCell(_cellEntities[EnKey(newPos)]);
            fakeItem.ReplaceGridPosition(newPos);
        }
    }

    private Vector2Int FindTargetCellToStand(Vector2Int pos)
    {
        for (int y = pos.y - 1; y >= 0; y--)
        {
            var cell = _cellEntities[EnKey(new Vector2Int(pos.x, y))];

            if (!cell.canAcceptFall.Value)
            {
                return new Vector2Int(pos.x, y);
            }
        }

        return new Vector2Int(pos.x, -1);
    }

    private bool IsSuitableMulti(Vector2Int pos, List<Vector2Int> currentPositions)
    {
        if (currentPositions.Contains(pos))
        {
            return true;
        }

        if (pos.y < 0)
            return false;

        if (pos.y >= _height)
            return false;

        var key = EnKey(pos);
        if (_cellEntities[key].cellState.Value == CellState.Full)
            return false;


        if (_cellEntities[key].cellState.Value == CellState.Invalid)
            return false;

        if (!_cellEntities[key].canAcceptFall.Value)
            return false;

        return true;
    }

    private bool CanFall(Vector2Int curPos, Vector2Int targetPos)
    {
        var key = EnKey(curPos);
        if (!_cellEntities[key].cell.CanLetItemFall)
            return false;

        if (targetPos.y < 0)
            return false;

        if (targetPos.y >= _height)
            return false;

        key = EnKey(targetPos);
        if (_cellEntities[key].cellState.Value == CellState.Full)
            return false;

        if (_cellEntities[key].cellState.Value == CellState.Invalid)
            return false;

        if (!_cellEntities[key].canAcceptFall.Value)
            return false;

        if (_cellEntities[key].hasItemReservation) return false;

        return true;
    }

    private int EnKey(Vector2Int pos)
    {
        return _width * pos.y + pos.x;
    }

    private void ItemEnteredCell(GameEntity cell)
    {
        cell.ReplaceCellState(CellState.Full);
        cell.ReplaceCanAcceptFall(false, cell.canAcceptFall.Counter);
        var oldCell = cell.cell;
        cell.ReplaceCell(oldCell.CanLetItemActivate, oldCell.CanLetItemFall);
    }

    private void ItemLeftCell(GameEntity cell)
    {
        var newCell = cell.cell;
        cell.ReplaceCellState(CellState.Empty);
        if (cell.canAcceptFall.Counter == 0)
        {
            cell.canAcceptFall.Value = true;
        }

        cell.ReplaceCell(newCell.CanLetItemActivate, newCell.CanLetItemFall);
    }
}