public abstract partial class ItemTypeInBoard
{
    private class GumType : ItemTypeInBoard
    {
        public GumType() : base(12, "Gum")
        {
        }

        public override ItemType ItemType => ItemType.Gum;
        public override GoalType GoalType => GoalType.Gum;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.ReplaceLayer(1);
            entity.isCanFall = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isFallStateChanging = true;
            entity.isCollectedAtEnd = true;
        }
    }
}