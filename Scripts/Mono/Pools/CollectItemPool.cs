using System.Collections.Generic;
using UnityEngine;

public class CollectItemPool : Pool<CollectItemPoolItems, CollectItemType, CollectItemView>
{

    protected override int GetInitialPoolCount(CollectItemType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<CollectItemType, CollectItemView>
        {
            {CollectItemType.Toast, items.toast},
            {CollectItemType.Bee, items.bee},
            {CollectItemType.ColorCube, items.colorCube},
            {CollectItemType.Toy, items.toy},
            {CollectItemType.CandyMonster, items.candyMonster},
            {CollectItemType.PinWheel, items.pinWheel},
            {CollectItemType.Ufo, items.ufo},
            {CollectItemType.Duck, items.duck},
        };
    }

    protected override void DeActivateItem(CollectItemView collectItem)
    {
        collectItem.SetParent(poolParent);
        collectItem.DeActivate();
        collectItem.StopTimedThings();
    }

    protected override void RefreshItem(CollectItemView collectItem, Vector2 pos)
    {
        collectItem.Refresh();
        collectItem.SetParentToRoot();
        collectItem.SetPosition(pos);
    }

    protected override void RenameItem(CollectItemView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(CollectItemView view)
    {
        return view.gameObject.name;
    }
}

public class CollectItemType : Enumeration
{
    public static readonly CollectItemType Toast = new CollectItemType(1, "Toast", 21);
    public static readonly CollectItemType Bee = new CollectItemType(2, "Bee", 21);
    public static readonly CollectItemType ColorCube = new CollectItemType(3, "ColorCube", 21);
    public static readonly CollectItemType Toy = new CollectItemType(4, "Toy", 21);
    public static readonly CollectItemType CandyMonster = new CollectItemType(5, "CandyMonster", 21);
    public static readonly CollectItemType PinWheel = new CollectItemType(6, "PinWheel", 21);
    public static readonly CollectItemType Ufo = new CollectItemType(7, "Ufo", 21);
    public static readonly CollectItemType Duck = new CollectItemType(8, "Duck", 21);

    public readonly int InitialPoolAmount;

    private CollectItemType(int id, string name, int initialPoolAmount)
        : base(id, name)
    {
        InitialPoolAmount = initialPoolAmount;
    }
}

namespace CollectItemPoolExtensions
{
    public static class MyExtensions
    {
        public static CollectItemView Spawn(this CollectItemType type, Vector2 pos)
        {
            return CollectItemPool.Instance.Spawn(type, pos);
        }

        public static void Destroy(this CollectItemView collectItem)
        {
            CollectItemPool.Instance.Destroy(collectItem);
        }
    }
}