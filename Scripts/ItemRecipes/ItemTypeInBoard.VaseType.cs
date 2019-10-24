public abstract partial class ItemTypeInBoard
{
    private class VaseType : ItemTypeInBoard
    {
        public VaseType() : base(119, "Vase")
        {
        }

        public override ItemType ItemType => ItemType.Vase;
        public override GoalType GoalType => GoalType.Vase;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(2);
            entity.isRemoverSensitive = true;
            entity.isCanFall = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isSubItem = true;
        }
    }
}