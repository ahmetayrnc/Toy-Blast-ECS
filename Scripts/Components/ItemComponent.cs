using System;
using System.Linq;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game]
public class ItemComponent : IComponent
{
}


[Game, Event(EventTarget.Self)]
public class ItemReservationComponent : IComponent
{
    [PrimaryEntityIndex] public int ReservationId;
    [PrimaryEntityIndex] public int ItemId;
    public ItemTypeInBoard ItemType;
}

[Game]
public class ItemReservationCompletedComponent : IComponent
{
}

namespace ItemExtensions
{
    public static class MyExtensions
    {
        public static GameEntity GetItemWithPosition(this GameContext game, Vector2Int pos)
        {
            var possibleItem = game.GetEntitiesWithGridPosition(pos).Where(e => e.isItem || e.hasFakeItem);

            var gameEntities = possibleItem as GameEntity[] ?? possibleItem.ToArray();

            if (!gameEntities.Any()) return null;

            var item = gameEntities.First();

            if (item.hasFakeItem)
            {
                return game.GetEntityWithId(item.fakeItem.RealItemId);
            }

            if (item.isItem)
            {
                return item;
            }

            return null;
        }

        public static GameEntity GetItemWithPosition(this GameContext game, Tuple<int, int> tuplePos)
        {
            var pos = new Vector2Int(tuplePos.Item1, tuplePos.Item2);
            var possibleItem = game.GetEntitiesWithGridPosition(pos).Where(e => e.isItem || e.hasFakeItem);

            var gameEntities = possibleItem as GameEntity[] ?? possibleItem.ToArray();

            if (!gameEntities.Any()) return null;

            var item = gameEntities.First();

            if (item.hasFakeItem)
            {
                return game.GetEntityWithId(item.fakeItem.RealItemId);
            }

            if (item.isItem)
            {
                return item;
            }

            return null;
        }
    }
}