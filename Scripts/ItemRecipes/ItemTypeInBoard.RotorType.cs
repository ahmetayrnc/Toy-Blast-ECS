public abstract partial class ItemTypeInBoard
{
    private abstract class RotorType : ItemTypeInBoard
    {
        public static readonly RotorType VerticalRotorType = new VerticalRotor();
        public static readonly RotorType HorizontalRotorType = new HorizontalRotor();

        private RotorType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Rotor;
        public override GoalType GoalType => GoalType.Rotor;

        private void CreateRotor(GameEntity entity, Axis axis)
        {
            entity.AddItemAxis(axis);
            entity.AddMatchType(MatchType.Positive);
            entity.AddHint(HintType.None);
            entity.AddMatchGroup(-1, 0);
            entity.isCanBeActivatedByTouch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isInnerMatchItem = true;
            entity.isPositiveItem = true;
        }

        private class VerticalRotor : RotorType
        {
            protected internal VerticalRotor() : base(37, "VerticalRotor")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateRotor(entity, Axis.Vertical);
            }
        }

        private class HorizontalRotor : RotorType
        {
            protected internal HorizontalRotor() : base(35, "HorizontalRotor")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateRotor(entity, Axis.Horizontal);
            }
        }
    }
}