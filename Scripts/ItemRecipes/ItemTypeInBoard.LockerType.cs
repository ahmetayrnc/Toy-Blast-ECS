public abstract partial class ItemTypeInBoard
{
    private class LockerType : ItemTypeInBoard
    {
        public LockerType() : base(16, "Locker")
        {
        }

        public override ItemType ItemType => ItemType.Locker;
        public override GoalType GoalType => GoalType.Locker;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(1);
            entity.isRemoverSensitive = true;
            entity.isLayerTypeChanging = true;
        }
    }
}