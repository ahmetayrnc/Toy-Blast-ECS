public abstract partial class ItemTypeInBoard
{
    private class SnowmanType : ItemTypeInBoard
    {
        public SnowmanType() : base(113, "Snowman")
        {
        }

        public override ItemType ItemType => ItemType.Snowman;
        public override GoalType GoalType => GoalType.Snowman;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(3);
            entity.isRemoverSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isCollectedAtEnd = true;
        }
    }
}