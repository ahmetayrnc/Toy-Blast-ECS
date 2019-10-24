public abstract partial class ItemTypeInBoard
{
    private class DishwasherType : ItemTypeInBoard
    {
        public DishwasherType() : base(188, "Dishwasher")
        {
        }

        public override ItemType ItemType => ItemType.Dishwasher;
        public override GoalType GoalType => GoalType.Dishwasher;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(8);
            entity.isMultiBlock = true;
            entity.isRemoverSensitive = true;
            entity.isLayerTypeChanging = true;
            entity.isRemoverSensitivityChanging = true;
            entity.isCantHaveBubbleOnTop = true;

            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M2X2);
        }
    }
}