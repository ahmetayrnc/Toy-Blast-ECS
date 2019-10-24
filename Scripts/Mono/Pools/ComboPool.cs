using System.Collections.Generic;
using UnityEngine;

public class ComboPool : Pool<ComboPoolItems, ComboType, ComboView>
{
    protected override int GetInitialPoolCount(ComboType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<ComboType, ComboView>
        {
            {ComboType.RotorRotor, items.rotorRotor},
            {ComboType.TntRotor, items.tntRotor},
            {ComboType.TntTnt, items.tntTnt},
            {ComboType.PuzzleCombo, items.puzzleCombo},
            {ComboType.PuzzlePuzzle, items.puzzlePuzzle},
        };
    }

    protected override void DeActivateItem(ComboView view)
    {
        view.UnlinkItem();
        view.DeActivate();
    }

    protected override void RefreshItem(ComboView view, Vector2 pos)
    {
    }

    protected override void RenameItem(ComboView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(ComboView view)
    {
        return view.gameObject.name;
    }
}

namespace ComboPoolExtensions
{
    public static class MyExtensions
    {
        public static ComboView Spawn(this ComboType type)
        {
            return ComboPool.Instance.Spawn(type, Vector3.zero);
        }

        public static void Destroy(this ComboView combo)
        {
            ComboPool.Instance.Destroy(combo);
        }
    }
}