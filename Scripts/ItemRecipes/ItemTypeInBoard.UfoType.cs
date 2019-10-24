public abstract partial class ItemTypeInBoard
{
    private abstract class UfoType : ItemTypeInBoard
    {
        public static readonly UfoType MActiveUfo = new ActiveUfoType();
        public static readonly UfoType MInActiveUfo = new InActiveUfoType();

        private UfoType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Ufo;
        public override GoalType GoalType => GoalType.Ufo;

        private void CreateUfo(GameEntity entity, int layer)
        {
            entity.ReplaceLayer(layer);
            entity.isTurnSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;

            entity.AddConsecutionSensitive(-1);
        }

        private class ActiveUfoType : UfoType
        {
            protected internal ActiveUfoType() : base(-2, "ActiveUfo")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateUfo(entity, 0);
            }
        }

        private class InActiveUfoType : UfoType
        {
            protected internal InActiveUfoType() : base(88, "InActiveUfo")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateUfo(entity, 1);
            }
        }
    }
}