public abstract partial class ItemTypeInBoard
{
    private class ChickType : ItemTypeInBoard
    {
        public ChickType() : base(132, "Chick")
        {
        }

        public override ItemType ItemType => ItemType.Chick;
        public override GoalType GoalType => GoalType.Chick;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(3);
            entity.isRemoverSensitive = true;
            entity.isCanFall = true;
            entity.isCanBeActivatedByNearMatch = true;
        }
    }
}