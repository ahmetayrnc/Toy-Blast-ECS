using System;
using UnityEngine;

public class CellHelper
{
//    static CellHelper()
//    {
//    }
//
//    private CellHelper()
//    {
//    }

//    public static CellHelper Instance { get; } = new CellHelper();

    public static void BlockFallAt(GameEntity cell)
    {
        cell.canAcceptFall.Counter++;
        cell.canAcceptFall.Value = false;
    }

    public static void UnBlockFallAt(GameEntity cell)
    {
        cell.canAcceptFall.Counter--;
        if (cell.canAcceptFall.Counter == 0)
        {
            cell.canAcceptFall.Value = true;
        }
    }

    public static void BlockFallAt(Vector2Int pos)
    {
        var contexts = Contexts.sharedInstance;
        var cell = contexts.game.GetEntityWithCellId(new Tuple<int, int>(pos.x, pos.y));

        if (cell == null) return;

        BlockFallAt(cell);
    }

    public static void UnBlockFallAt(Vector2Int pos)
    {
        var contexts = Contexts.sharedInstance;
        var cell = contexts.game.GetEntityWithCellId(new Tuple<int, int>(pos.x, pos.y));

        if (cell == null) return;

        UnBlockFallAt(cell);
    }
}