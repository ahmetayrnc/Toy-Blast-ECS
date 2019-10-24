public abstract partial class ItemTypeInBoard
{
    private abstract class CuckooType : ItemTypeInBoard
    {
        public static readonly CuckooType MActiveCuckoo = new ActiveCuckooType();
        public static readonly CuckooType MInActiveCuckoo = new InActiveCuckooType();

        private CuckooType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Cuckoo;
        public override GoalType GoalType => GoalType.Cuckoo;

        private void CreateCuckoo(GameEntity entity, int layer)
        {
            entity.ReplaceLayer(layer);
            entity.isTurnSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;

            entity.AddConsecutionSensitive(-1);
        }

        private class ActiveCuckooType : CuckooType
        {
            protected internal ActiveCuckooType() : base(-2, "ActiveCuckoo")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCuckoo(entity, 0);
            }
        }

        private class InActiveCuckooType : CuckooType
        {
            protected internal InActiveCuckooType() : base(115, "InActiveCuckoo")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCuckoo(entity, 1);
            }
        }
    }
}