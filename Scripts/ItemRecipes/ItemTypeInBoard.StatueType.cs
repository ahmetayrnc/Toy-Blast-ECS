public abstract partial class ItemTypeInBoard
{
    private class StatueType : ItemTypeInBoard
    {
        public StatueType() : base(156, "Statue")
        {
        }

        public override ItemType ItemType => ItemType.Statue;
        public override GoalType GoalType => GoalType.Statue;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(2);
            entity.isMultiBlock = true;
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
            entity.isCantHaveBubbleOnTop = true;

            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M2X2);
        }
    }
}