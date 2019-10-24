public abstract partial class ItemTypeInBoard
{
    private abstract class LegoType : ItemTypeInBoard
    {
        public static readonly LegoType MLego0 = new Lego0Type();
        public static readonly LegoType MLego1 = new Lego1Type();
        public static readonly LegoType MLego2 = new Lego2Type();
        public static readonly LegoType MLego3 = new Lego3Type();
        public static readonly LegoType MLego4 = new Lego4Type();

        private LegoType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Lego;
        public override GoalType GoalType => GoalType.Lego;

        private void CreateLego(GameEntity entity, int layer)
        {
            entity.ReplaceLayer(layer);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
        }

        private class Lego0Type : LegoType
        {
            protected internal Lego0Type() : base(33, "Lego0")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateLego(entity, 0);
            }
        }

        private class Lego1Type : LegoType
        {
            protected internal Lego1Type() : base(53, "Lego1")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateLego(entity, 1);
            }
        }

        private class Lego2Type : LegoType
        {
            protected internal Lego2Type() : base(54, "Lego2")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateLego(entity, 2);
            }
        }

        private class Lego3Type : LegoType
        {
            protected internal Lego3Type() : base(73, "Lego3")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateLego(entity, 3);
            }
        }

        private class Lego4Type : LegoType
        {
            protected internal Lego4Type() : base(74, "Lego4")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateLego(entity, 4);
            }
        }
    }
}