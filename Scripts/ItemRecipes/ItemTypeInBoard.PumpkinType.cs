public abstract partial class ItemTypeInBoard
{
    private class PumpkinType : ItemTypeInBoard
    {
        public PumpkinType() : base(13, "Pumpkin")
        {
        }

        public override ItemType ItemType => ItemType.Pumpkin;
        public override GoalType GoalType => GoalType.Pumpkin;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(2);
            entity.isCanFall = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
        }
    }
}