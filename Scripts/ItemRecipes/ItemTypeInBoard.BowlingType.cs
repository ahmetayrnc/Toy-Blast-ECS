public abstract partial class ItemTypeInBoard
{
    private class BowlingType : ItemTypeInBoard
    {
        public BowlingType() : base(78, "Bowling")
        {
        }

        public override ItemType ItemType => ItemType.Bowling;
        public override GoalType GoalType => GoalType.BowlingPin;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(9);
            entity.isCanBeActivatedByNearMatch = true;
            entity.isMultiBlock = true;
            entity.isCantHaveBubbleOnTop = true;
            entity.isCollectedAtLayer = true;
            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M2X2);
        }
    }
}