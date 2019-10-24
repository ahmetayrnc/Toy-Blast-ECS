public abstract partial class ItemTypeInBoard
{
    private class SafeType : ItemTypeInBoard
    {
        public SafeType() : base(131, "Safe")
        {
        }
        
        public override ItemType ItemType => ItemType.Safe;
        public override GoalType GoalType => GoalType.Safe;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(1);
            entity.isRemoverSensitive = true;
            entity.isLayerTypeChanging = true;
            entity.isFallStateChanging = true;
        }
    }
}