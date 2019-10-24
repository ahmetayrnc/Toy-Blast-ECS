using System.Collections.Generic;
using UnityEngine;

public class CellItemPool : Pool<CellItemPoolItems, CellItemType, CellItemView>
{
    protected override int GetInitialPoolCount(CellItemType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<CellItemType, CellItemView>
        {
            {CellItemType.Bubble, items.bubble},
            {CellItemType.Cage, items.cage},
            {CellItemType.Curtain, items.curtain},
        };
    }

    protected override void DeActivateItem(CellItemView view)
    {
        view.UnlinkItem();
        view.DeActivate();
    }

    protected override void RefreshItem(CellItemView view, Vector2 pos)
    {
    }

    protected override void RenameItem(CellItemView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(CellItemView view)
    {
        return view.gameObject.name;
    }
}

namespace CellItemPoolExtensions
{
    public static class MyExtensions
    {
        public static CellItemView Spawn(this CellItemType type)
        {
            return CellItemPool.Instance.Spawn(type, Vector3.zero);
        }

        public static void Destroy(this CellItemView cellItem)
        {
            CellItemPool.Instance.Destroy(cellItem);
        }
    }
}