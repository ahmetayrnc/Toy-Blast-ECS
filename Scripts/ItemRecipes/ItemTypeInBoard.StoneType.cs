public abstract partial class ItemTypeInBoard
{
    private class StoneType : ItemTypeInBoard
    {
        public StoneType() : base(134, "Stone")
        {
        }

        public override ItemType ItemType => ItemType.Stone;
        public override GoalType GoalType => GoalType.Stone;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(1);
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isLayerTypeChanging = true;
        }
    }
}