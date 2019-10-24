using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private class PinWheelType : ItemTypeInBoard
    {
        public PinWheelType() : base(75, "PinWheel")
        {
        }

        public override ItemType ItemType => ItemType.PinWheel;
        public override GoalType GoalType => GoalType.PinWheel;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(3);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;

            var colors = new Dictionary<Color, int>
            {
                [Color.Blue] = 1, [Color.Green] = 1, [Color.Red] = 1, [Color.Yellow] = 1
            };
            entity.AddColorSensitive(colors);
        }
    }
}