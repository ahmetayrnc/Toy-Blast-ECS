public abstract partial class ItemTypeInBoard
{
    private class PotionType : ItemTypeInBoard
    {
        public PotionType() : base(112, "Potion")
        {
        }

        public override ItemType ItemType => ItemType.Potion;
        public override GoalType GoalType => GoalType.Potion;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(1);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCanFall = true;
            entity.isColorCopying = true;
            entity.isCollectedAtEnd = true;
        }
    }
}