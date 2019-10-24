using Entitas;
using UnityEngine;

public class WillSpawnItemComponent : IComponent
{
    public ItemTypeInBoard ItemType;
    public Vector2Int GridPosition;
    public float WorldY;
}

public class WillFillComponent : IComponent
{
    public float Speed;
}