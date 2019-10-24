public abstract partial class ItemTypeInBoard
{
    private abstract class ToyType : ItemTypeInBoard
    {
        public static readonly ToyType MToy0 = new Toy0Type();
        public static readonly ToyType MToy1 = new Toy1Type();
        public static readonly ToyType MToy2 = new Toy2Type();
        public static readonly ToyType MToy3 = new Toy3Type();
        public static readonly ToyType MToy4 = new Toy4Type();
        public static readonly ToyType MToy5 = new Toy5Type();
        public static readonly ToyType MToy6 = new Toy6Type();
        public static readonly ToyType MToy7 = new Toy7Type();

        private ToyType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Toy;

        private void CreateToy(GameEntity entity, int variant)
        {
            entity.AddVariant(variant);
            entity.isCanBeActivatedByBottom = true;
            entity.isCantBeActivated = true;
            entity.isCanFall = true;
            entity.isCollectedAtEnd = true;
        }

        private class Toy0Type : ToyType
        {
            protected internal Toy0Type() : base(36, "Toy0")
            {
            }

            public override GoalType GoalType => GoalType.Toy0;

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 0);
            }
        }

        private class Toy1Type : ToyType
        {
            protected internal Toy1Type() : base(61, "Toy1")
            {
            }

            public override GoalType GoalType => GoalType.Toy1;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 1);
            }
        }

        private class Toy2Type : ToyType
        {
            protected internal Toy2Type() : base(63, "Toy2")
            {
            }

            public override GoalType GoalType => GoalType.Toy2;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 2);
            }
        }

        private class Toy3Type : ToyType
        {
            protected internal Toy3Type() : base(62, "Toy3")
            {
            }

            public override GoalType GoalType => GoalType.Toy3;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 3);
            }
        }

        private class Toy4Type : ToyType
        {
            protected internal Toy4Type() : base(60, "Toy4")
            {
            }

            public override GoalType GoalType => GoalType.Toy4;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 4);
            }
        }

        private class Toy5Type : ToyType
        {
            protected internal Toy5Type() : base(64, "Toy5")
            {
            }

            public override GoalType GoalType => GoalType.Toy5;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 5);
            }
        }

        private class Toy6Type : ToyType
        {
            protected internal Toy6Type() : base(65, "Toy6")
            {
            }

            public override GoalType GoalType => GoalType.Toy6;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 6);
            }
        }

        private class Toy7Type : ToyType
        {
            protected internal Toy7Type() : base(80, "Toy7")
            {
            }

            public override GoalType GoalType => GoalType.Toy7;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateToy(entity, 7);
            }
        }
    }
}