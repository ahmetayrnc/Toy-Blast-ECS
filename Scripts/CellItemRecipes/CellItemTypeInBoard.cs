using System;
using UnityEngine;

public abstract partial class CellItemTypeInBoard
{
}

public abstract partial class CellItemTypeInBoard : Enumeration
{
    public static readonly CellItemTypeInBoard Empty = new EmptyType();
    public static readonly CellItemTypeInBoard Bubble = new BubbleType();
    public static readonly CellItemTypeInBoard Cage = new CageType();
    public static readonly CellItemTypeInBoard Curtain = new CurtainType();

    private CellItemTypeInBoard(int id, string name)
        : base(id, name)
    {
    }

    public abstract void Spawn(GameEntity entity);

    public bool HasCellItemType()
    {
        return CellItemType != null;
    }

    public abstract CellItemType CellItemType { get; }

    public abstract GoalType GoalType { get; }

    private static void AddGeneralComponents(GameEntity entity, CellItemType itemType,
        GoalType goalType)
    {
        var spawnItemInfo = entity.willSpawnCellItem;
        var pos = new Vector2(spawnItemInfo.GridX, spawnItemInfo.GridY);
        entity.AddPosition(pos);
        entity.isCellItem = true;
        entity.AddCellItemType(itemType);
        entity.AddGoalType(goalType);
        entity.AddGridPosition(new Vector2Int(spawnItemInfo.GridX, spawnItemInfo.GridY));
        entity.AddCellItemId(new Tuple<int, int>(spawnItemInfo.GridX, spawnItemInfo.GridY));
    }
}