using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private class FireworkType : ItemTypeInBoard
    {
        public FireworkType() : base(220, "Firework")
        {
        }

        public override ItemType ItemType => ItemType.Firework;
        public override GoalType GoalType => GoalType.Firework;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(3);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isMultiBlock = true;
            entity.isCantHaveBubbleOnTop = true;

            var colors = new Dictionary<Color, int>
            {
                [Color.Blue] = 1, [Color.Green] = 1, [Color.Red] = 1, [Color.Yellow] = 1
            };
            entity.AddColorSensitive(colors);

            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M2X2);
        }
    }
}