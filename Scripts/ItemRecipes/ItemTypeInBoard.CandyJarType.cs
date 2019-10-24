using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private abstract class CandyJarType : ItemTypeInBoard
    {
        public static readonly CandyJarType MBlueCandyJar = new BlueCandyJarType();
        public static readonly CandyJarType MGreenCandyJar = new GreenCandyJarType();
        public static readonly CandyJarType MRedCandyJar = new RedCandyJarType();
        public static readonly CandyJarType MYellowCandyJar = new YellowCandyJarType();
        public static readonly CandyJarType MPurpleCandyJar = new PurpleCandyJarType();
        public static readonly CandyJarType MOrangeCandyJar = new OrangeCandyJarType();

        private CandyJarType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.CandyJar;
        public override GoalType GoalType => GoalType.CandyJar;

        private void CreateCandyJar(GameEntity entity, Color color)
        {
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCanFall = true;
            entity.isCollectedAtEnd = true;

            var colors = new Dictionary<Color, int> {[color] = 1};
            entity.AddColorSensitive(colors);
            entity.AddColor(color);
        }

        private class BlueCandyJarType : CandyJarType
        {
            protected internal BlueCandyJarType() : base(28, "BlueCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Blue);
            }
        }

        private class GreenCandyJarType : CandyJarType
        {
            protected internal GreenCandyJarType() : base(29, "GreenCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Green);
            }
        }

        private class RedCandyJarType : CandyJarType
        {
            protected internal RedCandyJarType() : base(27, "RedCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Red);
            }
        }

        private class YellowCandyJarType : CandyJarType
        {
            protected internal YellowCandyJarType() : base(26, "YellowCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Yellow);
            }
        }

        private class PurpleCandyJarType : CandyJarType
        {
            protected internal PurpleCandyJarType() : base(30, "PurpleCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Purple);
            }
        }

        private class OrangeCandyJarType : CandyJarType
        {
            protected internal OrangeCandyJarType() : base(31, "OrangeCandyJar")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCandyJar(entity, Color.Orange);
            }
        }
    }
}