using System;
using System.Collections.Generic;
using System.Linq;
using TiledSharp;
using UnityEngine;

public static class LevelParser
{
    private const string ItemLayer = "items";
    private const string CellItemLayer = "cellItems";
    private const string MoveCount = "moves";
    private const string Group1 = "group1";
    private const string Group2 = "group2";
    private const string Group3 = "group3";
    private const string UseForDrop = "useForDrop";

    private static readonly HashSet<int> FakeItemIds = new HashSet<int>()
    {
        58, 59, 79, 136, 137, 157
    };

    private static readonly Dictionary<string, int> GroupNames = new Dictionary<string, int>
    {
        {Group1, 0},
        {Group2, 1},
        {Group3, 2},
    };

    public static Level ParseLevel(int levelNo)
    {
        //Get Map
        var levelStr = levelNo.ToString().PadLeft(5, '0');
        var map = new TmxMap($"{Application.streamingAssetsPath}/Levels/level_{levelStr}.tmx");

        //Get Level Info From Map
        var mapProperties = map.Properties.ToArray();
        var width = map.Width;
        var height = map.Height;
        var itemLayer = map.TileLayers.FirstOrDefault(layer => layer.Name == ItemLayer);
        var cellItemLayer = map.TileLayers.FirstOrDefault(layer => layer.Name == CellItemLayer);
        var groupLayers = new TmxObjectGroup[10];
        var groups = new Group[10];

        foreach (var temp in map.ObjectGroups)
        {
            if (!GroupNames.ContainsKey(temp.Name)) continue;

            groupLayers[GroupNames[temp.Name]] = temp;
        }

        //goals
        var goals = new Dictionary<GoalType, int>(4);
        var goalCount = 0;
        foreach (var pair in mapProperties)
        {
            if (goalCount >= 4) break;

            if (!Enum.TryParse(pair.Key, true, out GoalType goalType)) continue;
            if (!int.TryParse(pair.Value, out var goalAmount)) continue;

            goals.Add(goalType, goalAmount);
            goalCount++;
        }

        if (goalCount == 0)
        {
            throw new ApplicationException("No goal in level");
        }

        //move count
        var moveCountPair = mapProperties.FirstOrDefault(p => p.Key == MoveCount);
        if (!int.TryParse(moveCountPair.Value, out var moveCount))
        {
            throw new ApplicationException("No move count");
        }

        //items
        if (itemLayer == null)
        {
            throw new ApplicationException("Item layer is null");
        }

        var itemList = itemLayer.Tiles.Select(i =>
                FakeItemIds.Contains(i.Gid - 1)
                    ? ItemTypeInBoard.Empty
                    : Enumeration.FromValue<ItemTypeInBoard>(i.Gid - 1))
            .ToArray();

        var itemGrid = itemList.ToArray().Make2DArray(height, width);

        //cell items
        CellItemTypeInBoard[,] cellItemGrid;
        if (cellItemLayer == null)
        {
            cellItemGrid = new CellItemTypeInBoard[height, width].Populate(CellItemTypeInBoard.Empty);
        }
        else
        {
            cellItemGrid = cellItemLayer.Tiles.Select(i => Enumeration.FromValue<CellItemTypeInBoard>(i.Gid - 1))
                .ToArray().Make2DArray(height, width);
        }

        //groups
        if (groupLayers.All(g => g == null))
        {
            var items = new List<ItemTypeInBoard>
            {
                ItemTypeInBoard.BlueCube,
                ItemTypeInBoard.GreenCube,
                ItemTypeInBoard.RedCube,
                ItemTypeInBoard.YellowCube,
            };
            groups[0] = new Group
            {
                Items = items,
                UseForDrop = true
            };
            Debug.LogError("Could not find any Group. Created Temp group for drop");
        }
        else
        {
            for (var i = 0; i < groupLayers.Length; i++)
            {
                if (groupLayers[i] == null) continue;

                //group items
                if (groupLayers[i].Objects.Count <= 0)
                {
                    throw new ApplicationException(
                        $"There were no items in {groupLayers[i].Name}. Added temp items to group");
                }

                var items = groupLayers[i].Objects.ToList()
                    .Select(o => Enumeration.FromValue<ItemTypeInBoard>(o.Tile.Gid - 1)).ToList();

                //use for drop
                bool useForDrop;
                if (!groupLayers[i].Properties.ContainsKey(UseForDrop))
                {
                    useForDrop = false;
                }
                else
                {
                    bool.TryParse(groupLayers[i].Properties[UseForDrop], out useForDrop);
                }

                groups[i] = new Group
                {
                    Items = items,
                    UseForDrop = useForDrop,
                };
            }

            if (groups.All(g => g == null || !g.UseForDrop))
            {
                throw new ApplicationException($"Could not find any group for drop. Please add it");
            }
        }

        var level = new Level(width, height, itemGrid, cellItemGrid, goals, moveCount, groups);

        return level;
    }

    private static T[,] Make2DArray<T>(this IReadOnlyList<T> input, int height, int width)
    {
        var output = new T[height, width];
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }

        return output;
    }

    private static T[,] Populate<T>(this T[,] arr, T value)
    {
        for (var i = 0; i < arr.GetLength(0); i++)
        {
            for (var j = 0; j < arr.GetLength(1); j++)
            {
                arr[i, j] = value;
            }
        }

        return arr;
    }
}

public class Level
{
    public readonly ItemTypeInBoard[,] ItemGrid;
    public readonly CellItemTypeInBoard[,] CellItemGrid;
    public readonly Dictionary<GoalType, int> Goal;
    public readonly int Width;
    public readonly int Height;
    public readonly int MoveCount;
    public readonly Group[] Groups;

    public Level(
        int width,
        int height,
        ItemTypeInBoard[,] itemGrid,
        CellItemTypeInBoard[,] cellItemGrid,
        Dictionary<GoalType, int> goal,
        int moveCount, Group[] groups)
    {
        ItemGrid = itemGrid;
        CellItemGrid = cellItemGrid;
        MoveCount = moveCount;
        Groups = groups;
        Width = width;
        Height = height;
        Goal = goal;
    }
}

public class Group
{
    public List<ItemTypeInBoard> Items;
    public bool UseForDrop;
}