public abstract partial class ItemTypeInBoard
{
    private abstract class EggType : ItemTypeInBoard
    {
        public static readonly EggType MEgg0 = new Egg0Type();
        public static readonly EggType MEgg1 = new Egg1Type();

        private EggType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Egg;
        public override GoalType GoalType => GoalType.Egg;


        private void CreateEgg(GameEntity entity, int layer)
        {
            entity.ReplaceLayer(layer);
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCollectedAtEnd = true;
        }

        private class Egg0Type : EggType
        {
            protected internal Egg0Type() : base(-2, "Egg0")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateEgg(entity, 0);
            }
        }

        private class Egg1Type : EggType
        {
            protected internal Egg1Type() : base(14, "Egg1")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateEgg(entity, 1);
            }
        }
    }
}