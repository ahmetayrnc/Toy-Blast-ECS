using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private abstract class OrbType : ItemTypeInBoard
    {
        public static readonly OrbType MBlueOrb = new BlueOrbType();
        public static readonly OrbType MGreenOrb = new GreenOrbType();
        public static readonly OrbType MRedOrb = new RedOrbType();
        public static readonly OrbType MYellowOrb = new YellowOrbType();

        private OrbType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Orb;
        public override GoalType GoalType => GoalType.Orb;

        private void CreateOrb(GameEntity entity, Color color)
        {
            entity.ReplaceLayer(0);
            entity.isTurnSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCanFall = true;

            var colors = new Dictionary<Color, int>
            {
                [color] = 1
            };
            entity.AddColorSensitive(colors);

            var colorArray = new[]
            {
                Color.Blue,
                Color.Green,
                Color.Red,
                Color.Yellow,
                Color.Blue,
                Color.Green,
                Color.Red,
                Color.Yellow,
            };

            var colorQueue = new Queue<Color>();
            bool found = false;
            int count = 0;
            foreach (var tempColor in colorArray)
            {
                if (!found && color == tempColor)
                {
                    found = true;
                }

                if (!found)
                {
                    continue;
                }

                if (count >= 4)
                {
                    break;
                }

                colorQueue.Enqueue(tempColor);
                count++;
            }

            entity.AddColorChanging(colorQueue);

            entity.AddColor(color);
        }

        private class BlueOrbType : OrbType
        {
            protected internal BlueOrbType() : base(116, "BlueOrb")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateOrb(entity, Color.Blue);
            }
        }

        private class GreenOrbType : OrbType
        {
            protected internal GreenOrbType() : base(-1, "GreenOrb")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateOrb(entity, Color.Green);
            }
        }

        private class RedOrbType : OrbType
        {
            protected internal RedOrbType() : base(-1, "RedOrb")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateOrb(entity, Color.Red);
            }
        }

        private class YellowOrbType : OrbType
        {
            protected internal YellowOrbType() : base(-1, "YellowOrb")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateOrb(entity, Color.Yellow);
            }
        }
    }
}