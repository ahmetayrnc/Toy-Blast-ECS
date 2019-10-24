using System.Collections.Generic;
using UnityEngine;

public class ActiveRotorPool : Pool<ActiveRotorPoolItems, ActiveRotorType, ActiveRotorView>
{ 
    protected override int GetInitialPoolCount(ActiveRotorType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<ActiveRotorType, ActiveRotorView>
        {
            {ActiveRotorType.ActiveRotor, items.activeRotor},
        };
    }

    protected override void DeActivateItem(ActiveRotorView view)
    {
        view.UnlinkItem();
        view.DeActivate();
    }

    protected override void RefreshItem(ActiveRotorView view, Vector2 pos)
    {
    }

    protected override void RenameItem(ActiveRotorView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(ActiveRotorView view)
    {
        return view.gameObject.name;
    }
}

namespace ActiveRotorPoolExtensions
{
    public static class MyExtensions
    {
        public static ActiveRotorView Spawn(this ActiveRotorType type)
        {
            return ActiveRotorPool.Instance.Spawn(type, Vector3.zero);
        }

        public static void Destroy(this ActiveRotorView activeRotor)
        {
            ActiveRotorPool.Instance.Destroy(activeRotor);
        }
    }
}