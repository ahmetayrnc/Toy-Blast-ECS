using System;
using System.Collections.Generic;
using System.Linq;
using ItemExtensions;
using UnityEngine;
using Color = EntitasBlast.Color;
using Random = UnityEngine.Random;

public static class ActivatorHelper
{
    public static void TryActivateItemWithPositive(Vector2Int touchPos, int removerId, ActivationReason activationReason)
    {
        var contexts = Contexts.sharedInstance;
        if (!InBounds(contexts, touchPos))
        {
            return;
        }

        var cellItem = contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(touchPos.x, touchPos.y));
        var item = contexts.game.GetItemWithPosition(touchPos);

        if (cellItem != null && cellItem.isCanStopItemActivation)
        {
            cellItem.isWillBeDestroyed = true;
            return;
        }

        if (item == null)
        {
            return;
        }

        if (item.isCantBeActivated)
        {
            return;
        }

        var removerSet = item.removers.Set;

        if (removerSet.Contains(removerId))
        {
            return;
        }

        if (item.isRemoverSensitive)
        {
            removerSet.Add(removerId);
            item.ReplaceRemovers(removerSet);
        }

        if (item.hasColorSensitive)
        {
            ReduceColorSensitivityPositive(item);
        }

        if (item.isColorCopying && !item.hasColorSensitive)
        {
            var possibleColors = new[] {Color.Blue, Color.Red, Color.Green, Color.Yellow};
            var randColor = possibleColors.Shuffle().First();
            var colors = new Dictionary<Color, int>
            {
                [randColor] = 1
            };
            item.AddColorSensitive(colors);
            item.AddColor(randColor);
        }

        if (cellItem != null && cellItem.isCanBeActivatedByInnerMatch && item.isInnerMatchItem)
        {
            cellItem.isWillBeDestroyed = true;
        }

        ActivateItem(item, activationReason);
    }

    private static void ReduceColorSensitivityPositive(GameEntity item)
    {
        var colors = item.colorSensitive.colors;

        if (colors.ContainsKey(Color.Blue) && colors[Color.Blue] > 0)
        {
            colors[Color.Blue]--;
        }
        else if (colors.ContainsKey(Color.Green) && colors[Color.Green] > 0)
        {
            colors[Color.Green]--;
        }
        else if (colors.ContainsKey(Color.Orange) && colors[Color.Orange] > 0)
        {
            colors[Color.Orange]--;
        }
        else if (colors.ContainsKey(Color.Purple) && colors[Color.Purple] > 0)
        {
            colors[Color.Purple]--;
        }
        else if (colors.ContainsKey(Color.Red) && colors[Color.Red] > 0)
        {
            colors[Color.Red]--;
        }
        else if (colors.ContainsKey(Color.Yellow) && colors[Color.Yellow] > 0)
        {
            colors[Color.Yellow]--;
        }

        item.ReplaceColorSensitive(colors);
    }

    public static void TryActivateItemWithNear(Contexts contexts, Vector2Int pos, int removerId, Color color)
    {
        if (!InBounds(contexts, pos))
        {
            return;
        }

        var entitySet = contexts.game.GetEntitiesWithGridPosition(pos);
        GameEntity cellItem = null;
        GameEntity item = null;
        foreach (var entity in entitySet)
        {
            if (entity.isEnabled && entity.isCellItem)
            {
                cellItem = entity;
            }

            if (entity.isEnabled && entity.isItem)
            {
                item = entity;
            }

            if (entity.isEnabled && entity.hasFakeItem)
            {
                item = contexts.game.GetEntityWithId(entity.fakeItem.RealItemId);
            }
        }

        var hasCellItem = cellItem != null;
        var hasItem = item != null;

        if (hasCellItem && cellItem.isCanBeActivatedByNearMatch)
        {
            cellItem.isWillBeDestroyed = true;
            return;
        }

        if (hasCellItem && cellItem.isCanStopItemActivation)
        {
            return;
        }

        if (item == null || !item.isEnabled)
        {
            return;
        }

        if (item.isCantBeActivated)
        {
            return;
        }

        if (!item.isCanBeActivatedByNearMatch)
        {
            return;
        }

        var removerSet = item.removers.Set;

        if (removerSet.Contains(removerId))
        {
            return;
        }

        if (item.isRemoverSensitive)
        {
            removerSet.Add(removerId);
            item.ReplaceRemovers(removerSet);
        }

        if (item.hasColorSensitive)
        {
            var reduced = ReduceColorSensitivityWithNearMatch(item, color);
            if (!reduced)
            {
                return;
            }
        }

        if (item.isColorCopying && !item.hasColorSensitive)
        {
            var colors = new Dictionary<Color, int>
            {
                [color] = 1
            };
            item.AddColorSensitive(colors);
            item.AddColor(color);
        }

        ActivateItem(item, ActivationReason.NearMatch);
    }

    private static bool ReduceColorSensitivityWithNearMatch(GameEntity item, Color color)
    {
        var colors = item.colorSensitive.colors;
        bool reduced = false;

        if (colors.ContainsKey(color) && colors[color] > 0)
        {
            colors[color]--;
            reduced = true;
        }

        if (reduced)
        {
            item.ReplaceColorSensitive(colors);
        }

        return reduced;
    }

    public static void ActivateItem(GameEntity entity, ActivationReason reason)
    {
        Queue<ActivationReason> activationQueue;
        if (entity.hasActivation)
        {
            activationQueue = entity.activation.ActivationQueue;
            activationQueue.Enqueue(reason);
            entity.ReplaceActivation(activationQueue);
        }
        else
        {
            activationQueue = new Queue<ActivationReason>();
            activationQueue.Enqueue(reason);
            entity.AddActivation(activationQueue);
        }
    }

    private static bool InBounds(Contexts contexts, Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= contexts.game.board.Size.x)
            return false;

        if (pos.y < 0 || pos.y >= contexts.game.board.Size.y)
            return false;

        return true;
    }
}