using Entitas;
using ItemExtensions;
using UnityEngine;

public class BottomAccessSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    private GameEntity[,] _cells;

    private readonly IGroup<GameEntity> _cellGroup;
    private int _width;
    private int _height;

    public BottomAccessSystem(Contexts contexts)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.Cell);
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cells = new GameEntity[_width, _height];

        foreach (var e in _cellGroup)
        {
            var pos = e.gridPosition.value;
            _cells[pos.x, pos.y] = e;
            e.isHasAccessToBottom = true;
        }

        for (int x = 0; x < _width; x++)
        {
            if (!TryFindFirstBottomCellBlockingItem(x, out var blockingY)) continue;

            for (int y = blockingY; y < _height; y++)
            {
                _cells[x, y].isHasAccessToBottom = false;
            }
        }
    }

    private bool TryFindFirstBottomCellBlockingItem(int x, out int result)
    {
        for (int y = 0; y < _height; y++)
        {
            var item = _contexts.game.GetItemWithPosition(new Vector2Int(x, y));
            if (item == null) continue;
            if (!item.isBottomCellBlocking) continue;

            result = y;
            return true;
        }

        result = -1;
        return false;
    }
}