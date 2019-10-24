using UnityEngine;
using Color = EntitasBlast.Color;

public abstract partial class ItemTypeInBoard
{
    private abstract class ColorCubeType : ItemTypeInBoard
    {
        public static readonly ColorCubeType MBlueCube = new BlueColorCube();
        public static readonly ColorCubeType MGreenCube = new GreenColorCube();
        public static readonly ColorCubeType MRedCube = new RedColorCube();
        public static readonly ColorCubeType MYellowCube = new YellowColorCube();
//        public static readonly ColorCubeType RandomColorCubeInBoard = new RandomColorCube();

        private ColorCubeType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.ColorCube;

        private void CreateColorCube(GameEntity entity, MatchType matchType, Color color)
        {
            entity.AddColor(color);
            entity.AddHint(HintType.None);
            entity.AddMatchType(matchType);
            entity.AddMatchGroup(-1, 0);
            entity.isCanBeActivatedByTouch = true;
            entity.isCanFall = true;
            entity.isRemoverSensitive = true;
            entity.isInnerMatchItem = true;
            entity.isCanBeTargetedByGenerator = true;
            entity.isCollectedAtEnd = true;
        }

        private class GreenColorCube : ColorCubeType
        {
            protected internal GreenColorCube() : base(3, "GreenCube")
            {
            }

            public override GoalType GoalType => GoalType.GreenCube;

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorCube(entity, MatchType.GreenCube, Color.Green);
            }
        }

        private class BlueColorCube : ColorCubeType
        {
            protected internal BlueColorCube() : base(2, "BlueCube")
            {
            }
            public override GoalType GoalType => GoalType.BlueCube;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorCube(entity, MatchType.BlueCube, Color.Blue);
            }
        }

        private class RedColorCube : ColorCubeType
        {
            protected internal RedColorCube() : base(1, "RedCube")
            {
            }
            
            public override GoalType GoalType => GoalType.RedCube;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorCube(entity, MatchType.RedCube, Color.Red);
            }
        }

        private class YellowColorCube : ColorCubeType
        {
            protected internal YellowColorCube() : base(0, "YellowCube")
            {
            }
            
            public override GoalType GoalType => GoalType.YellowCube;


            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateColorCube(entity, MatchType.YellowCube, Color.Yellow);
            }
        }

//        private class RandomColorCube : ColorCubeType
//        {
//            protected internal RandomColorCube() : base(89, "RandomColorCube")
//            {
//            }
//
//            public override GoalType GoalType => GoalType.BlueCube;
//
//            public override void Spawn(GameEntity entity)
//            {
//                var randomId = Random.Range(0, 4);
//                switch (randomId)
//                {
//                    case 0:
//                        MBlueCube.Spawn(entity);
//                        return;
//                    case 1:
//                        MGreenCube.Spawn(entity);
//                        return;
//                    case 2:
//                        MRedCube.Spawn(entity);
//                        return;
//                    case 3:
//                        MYellowCube.Spawn(entity);
//                        return;
//                }
//            }
//        }
    }
}