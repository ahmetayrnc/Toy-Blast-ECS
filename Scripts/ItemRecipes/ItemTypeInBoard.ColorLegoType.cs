using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private abstract class ColorLegoType : ItemTypeInBoard
    {
        public static readonly ColorLegoType MBlueLego = new BlueLegoType();
        public static readonly ColorLegoType MGreenLego = new GreenLegoType();
        public static readonly ColorLegoType MOrangeLego = new OrangeLegoType();
        public static readonly ColorLegoType MPurpleLego = new PurpleLegoType();
        public static readonly ColorLegoType MRedLego = new RedLegoType();
        public static readonly ColorLegoType MYellowLego = new YellowLegoType();

        private ColorLegoType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.ColorLego;
        public override GoalType GoalType => GoalType.ColorLego;

        private void CreateColorLego(GameEntity entity, Color color)
        {
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;

            var colors = new Dictionary<Color, int> {[color] = 1};
            entity.AddColorSensitive(colors);
            entity.AddColor(color);
        }

        private class BlueLegoType : ColorLegoType
        {
            protected internal BlueLegoType() : base(130, "BlueLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Blue);
            }
        }

        private class GreenLegoType : ColorLegoType
        {
            protected internal GreenLegoType() : base(135, "GreenLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Green);
            }
        }

        private class OrangeLegoType : ColorLegoType
        {
            protected internal OrangeLegoType() : base(145, "OrangeLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Orange);
            }
        }

        private class PurpleLegoType : ColorLegoType
        {
            protected internal PurpleLegoType() : base(140, "PurpleLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Purple);
            }
        }

        private class RedLegoType : ColorLegoType
        {
            protected internal RedLegoType() : base(125, "RedLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Red);
            }
        }

        private class YellowLegoType : ColorLegoType
        {
            protected internal YellowLegoType() : base(120, "YellowLego")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorLego(entity, Color.Yellow);
            }
        }
    }
}