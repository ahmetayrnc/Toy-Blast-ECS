public abstract partial class ItemTypeInBoard
{
    private class MetalLegoType : ItemTypeInBoard
    {
        public MetalLegoType() : base(19, "MetalLego")
        {
        }

        public override ItemType ItemType => ItemType.MetalLego;
        public override GoalType GoalType => GoalType.MetalLego;

        public override void Spawn(GameEntity entity)
        {
            AddGeneralComponents(entity, ItemType, GoalType);
            entity.isRemoverSensitive = true;
            entity.isCollectedAtEnd = true;
        }
    }
}