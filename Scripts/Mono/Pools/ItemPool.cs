using System.Collections.Generic;
using UnityEngine;

public class ItemPool : Pool<ItemPoolItems, ItemType, ItemView>
{
    protected override int GetInitialPoolCount(ItemType type)
    {
        return type.InitialPoolAmount;
    }

    protected override void FillUpPairs()
    {
        Pairs = new Dictionary<ItemType, ItemView>
        {
            {ItemType.BeachBall, items.beachBall},
            {ItemType.ColorCube, items.colorCube},
            {ItemType.Bowling, items.bowling},
            {ItemType.CandyJar, items.candyJar},
            {ItemType.Chick, items.chick},
            {ItemType.Cocktail, items.cocktail},
            {ItemType.Cuckoo, items.cuckoo},
            {ItemType.ColorLego, items.colorLego},
            {ItemType.Dishwasher, items.dishwasher},
            {ItemType.Dragon, items.dragon},
            {ItemType.Duck, items.duck},
            {ItemType.Egg, items.egg},
            {ItemType.Firework, items.firework},
            {ItemType.Gum, items.gum},
            {ItemType.Lego, items.lego},
            {ItemType.Locker, items.locker},
            {ItemType.MetalLego, items.metalLego},
            {ItemType.Orb, items.orb},
            {ItemType.Piggy, items.piggy},
            {ItemType.PinWheel, items.pinWheel},
            {ItemType.Potion, items.potion},
            {ItemType.Pumpkin, items.pumpkin},
            {ItemType.Puzzle, items.puzzle},
            {ItemType.Rotor, items.rotor},
            {ItemType.Safe, items.safe},
            {ItemType.Snowman, items.snowman},
            {ItemType.Soap, items.soap},
            {ItemType.Statue, items.statue},
            {ItemType.Stone, items.stone},
            {ItemType.Tnt, items.tnt},
            {ItemType.Toaster, items.toaster},
            {ItemType.Toy, items.toy},
            {ItemType.Ufo, items.ufo},
            {ItemType.Vase, items.vase},
            {ItemType.Beehive, items.beehive},
            {ItemType.Washer, items.washer},
            {ItemType.Cannon, items.cannon},
            {ItemType.LaunchPad, items.launchPad},
            {ItemType.CandyMonster, items.candyMonster},
        };
    }

    protected override void DeActivateItem(ItemView view)
    {
        view.UnlinkItem();
        view.DeActivate();
    }

    protected override void RefreshItem(ItemView view, Vector2 pos)
    {
    }

    protected override void RenameItem(ItemView view, string newName)
    {
        view.SetName(newName);
    }

    protected override string NameOf(ItemView view)
    {
        return view.gameObject.name;
    }
}

namespace ItemPoolExtensions
{
    public static class MyExtensions
    {
        public static ItemView Spawn(this ItemType type)
        {
            return ItemPool.Instance.Spawn(type, Vector3.zero);
        }

        public static void Destroy(this ItemView collectItem)
        {
            ItemPool.Instance.Destroy(collectItem);
        }
    }
}