public abstract partial class ItemTypeInBoard
{
    private abstract class DuckType : ItemTypeInBoard
    {
        public static readonly DuckType MActiveDuck = new ActiveDuckType();
        public static readonly DuckType MInActiveDuck = new InActiveDuckType();

        private DuckType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Duck;
        public override GoalType GoalType => GoalType.Duck;

        private void CreateDuck(GameEntity entity, bool cantBeActivated)
        {
            entity.isTurnSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;

            if (cantBeActivated)
            {
                entity.isCantBeActivated = true;
            }
        }

        private class ActiveDuckType : DuckType
        {
            protected internal ActiveDuckType() : base(124, "ActiveDuck")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateDuck(entity, cantBeActivated: false);
            }
        }

        private class InActiveDuckType : DuckType
        {
            protected internal InActiveDuckType() : base(144, "InActiveDuck")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateDuck(entity, cantBeActivated: true);
            }
        }
    }
}