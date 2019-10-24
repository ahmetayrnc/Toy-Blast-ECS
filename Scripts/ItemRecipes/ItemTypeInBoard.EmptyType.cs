public abstract partial class ItemTypeInBoard
{
    private class EmptyType : ItemTypeInBoard
    {
        public EmptyType() : base(100, "Empty")
        {
        }

        public override ItemType ItemType { get; }
        public override GoalType GoalType { get; }

        public override void Spawn(GameEntity entity)
        {
            entity.isWillBeDestroyed = true;
        }
    }
}