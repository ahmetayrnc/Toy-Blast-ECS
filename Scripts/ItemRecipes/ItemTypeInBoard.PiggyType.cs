public abstract partial class ItemTypeInBoard
{
    private abstract class PiggyType : ItemTypeInBoard
    {
        public static readonly PiggyType MPiggy0 = new Piggy0Type();
        public static readonly PiggyType MPiggy1 = new Piggy1Type();

        private PiggyType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Piggy;
        public override GoalType GoalType => GoalType.Piggy;

        private void CreatePiggy(GameEntity entity, int layer)
        {
            entity.ReplaceLayer(layer);
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
        }

        private class Piggy0Type : PiggyType
        {
            protected internal Piggy0Type() : base(15, "Piggy0")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePiggy(entity, 0);
            }
        }

        private class Piggy1Type : PiggyType
        {
            protected internal Piggy1Type() : base(38, "Piggy1")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreatePiggy(entity, 1);
            }
        }
    }
}