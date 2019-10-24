using System.Collections.Generic;
using UnityEngine;

public class ThrowItemPool : Pool<ThrowItemPoolItems, ThrowItemType, ThrowItemView>
{
    protected override int GetInitialPoolCount(ThrowItemType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<ThrowItemType, ThrowItemView>
        {
            {ThrowItemType.Bubble, items.bubble},
            {ThrowItemType.BeachBall, items.beachBall},
            {ThrowItemType.Rocket, items.rocket},
        };
    }

    protected override void DeActivateItem(ThrowItemView collectItem)
    {
        collectItem.SetParent(poolParent);
        collectItem.DeActivate();
        collectItem.StopTimedThings();
    }

    protected override void RefreshItem(ThrowItemView view, Vector2 pos)
    {
        view.Refresh();
        view.SetParentToRoot();
        view.SetPosition(pos);
    }

    protected override void RenameItem(ThrowItemView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(ThrowItemView view)
    {
        return view.gameObject.name;
    }
}

public class ThrowItemType : Enumeration
{
    public static readonly ThrowItemType Bubble = new ThrowItemType(1, "Bubble", 21);
    public static readonly ThrowItemType BeachBall = new ThrowItemType(2, "BeachBall", 21);
    public static readonly ThrowItemType Rocket = new ThrowItemType(3, "Rocket", 21);

    public readonly int InitialPoolAmount;

    private ThrowItemType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}

namespace ThrowItemPoolExtensions
{
    public static class MyExtensions
    {
        public static ThrowItemView Spawn(this ThrowItemType type, Vector2 pos)
        {
            return ThrowItemPool.Instance.Spawn(type, pos);
        }

        public static void Destroy(this ThrowItemView collectItem)
        {
            ThrowItemPool.Instance.Destroy(collectItem);
        }
    }
}