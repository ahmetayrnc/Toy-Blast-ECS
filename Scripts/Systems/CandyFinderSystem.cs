using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class CandyFinderSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    private GameEntity[,] _candies;
    private List<GameEntity> _candyHeads;

    private readonly IGroup<GameEntity> _candyGroup;
    private readonly IGroup<GameEntity> _candyHeadGroup;
    private int _width;
    private int _height;

    public CandyFinderSystem(Contexts contexts)
    {
        _contexts = contexts;
        _candyHeadGroup = contexts.game.GetGroup(GameMatcher.Item);
        _candyGroup = contexts.game.GetGroup(GameMatcher.CandyFakeItem);
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _candies = new GameEntity[_width, _height];
        _candyHeads = new List<GameEntity>();

        foreach (var candyHead in _candyHeadGroup.GetEntities()
            .Where(c => Equals(c.itemType.Value, ItemType.CandyMonster)))
        {
            _candyHeads.Add(candyHead);
        }

        foreach (var candy in _candyGroup)
        {
            var pos = candy.gridPosition.value;
            _candies[pos.x, pos.y] = candy;
        }

        foreach (var candyHead in _candyHeads)
        {
            DetermineCandyDirectionAndLength(candyHead);
        }
    }

    private void DetermineCandyDirectionAndLength(GameEntity candyHead)
    {
        var pos = candyHead.gridPosition.value;
        CalculateCandyLength(_candies, pos.x, pos.y, out var length, out var itemDirection);

        if (length == 0)
        {
            Debug.LogError("There is an empty candy head in the level");
        }

        CreateCandyMonster(candyHead, length, itemDirection);
    }

    private void CreateCandyMonster(GameEntity candyHead, int length, ItemDirection itemDirection)
    {
        var orgX = candyHead.gridPosition.value.x;
        var orgY = candyHead.gridPosition.value.y;
        var fakeItemIds = new Stack<int>();
        switch (itemDirection)
        {
            case ItemDirection.Left:
                for (int x = orgX - 1; x >= orgX - length; x--)
                {
                    var fake = _candies[x, orgY];
                    ConvertCandyFakeItem(fake, candyHead, fakeItemIds);
                }

                break;

            case ItemDirection.Down:
                for (int y = orgY - 1; y >= orgY - length; y--)
                {
                    var fake = _candies[orgX, y];
                    ConvertCandyFakeItem(fake, candyHead, fakeItemIds);
                }

                break;

            case ItemDirection.Right:
                for (int x = orgX + 1; x <= orgX + length; x++)
                {
                    var fake = _candies[x, orgY];
                    ConvertCandyFakeItem(fake, candyHead, fakeItemIds);
                }

                break;

            case ItemDirection.Up:
                for (int y = orgY + 1; y <= orgY + length; y++)
                {
                    var fake = _candies[orgX, y];
                    ConvertCandyFakeItem(fake, candyHead, fakeItemIds);
                }

                break;
        }

        candyHead.ReplaceLayer(length - 1);
        candyHead.AddItemDirection(itemDirection);
        candyHead.AddOrderedFakeItems(fakeItemIds);
    }

    private void ConvertCandyFakeItem(GameEntity fake, GameEntity candyHead, Stack<int> fakeItemIds)
    {
        fake.AddFakeItem(candyHead.id.Value);
        fake.AddId(IdHelper.GetNewId());
        fake.isCandyFakeItem = false;
        fake.RemoveItemDirection();
        fakeItemIds.Push(fake.id.Value);
    }

    private void CalculateCandyLength(GameEntity[,] itemGrid, int orgX, int orgY, out int length,
        out ItemDirection itemDirection)
    {
        //left
        int candyLength = 0;
        for (int x = orgX - 1; x >= 0; x--)
        {
            if (itemGrid[x, orgY] == null) break;
            if (!itemGrid[x, orgY].isCandyFakeItem) break;
            if (itemGrid[x, orgY].itemDirection.Value != ItemDirection.Left) break;

            candyLength++;
        }

        if (candyLength > 0)
        {
            length = candyLength;
            itemDirection = ItemDirection.Left;
            return;
        }

        //right
        for (int x = orgX + 1; x < _width; x++)
        {
            if (itemGrid[x, orgY] == null) break;
            if (!itemGrid[x, orgY].isCandyFakeItem) break;
            if (itemGrid[x, orgY].itemDirection.Value != ItemDirection.Right) break;

            candyLength++;
        }

        if (candyLength > 0)
        {
            length = candyLength;
            itemDirection = ItemDirection.Right;
            return;
        }

        //up
        for (int y = orgY + 1; y < _height; y++)
        {
            if (itemGrid[orgX, y] == null) break;
            if (!itemGrid[orgX, y].isCandyFakeItem) break;
            if (itemGrid[orgX, y].itemDirection.Value != ItemDirection.Up) break;

            candyLength++;
        }

        if (candyLength > 0)
        {
            length = candyLength;
            itemDirection = ItemDirection.Up;
            return;
        }

        //down
        for (int y = orgY - 1; y >= 0; y--)
        {
            if (itemGrid[orgX, y] == null) break;
            if (!itemGrid[orgX, y].isCandyFakeItem) break;
            if (itemGrid[orgX, y].itemDirection.Value != ItemDirection.Down) break;

            candyLength++;
        }

        length = candyLength;
        itemDirection = ItemDirection.Down;
    }
}