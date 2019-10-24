using System.Collections.Generic;
using EntitasBlast;

public abstract partial class ItemTypeInBoard
{
    private abstract class CocktailType : ItemTypeInBoard
    {
        public static readonly CocktailType MBlueCocktail = new BlueCocktailType();
        public static readonly CocktailType MGreenCocktail = new GreenCocktailType();
        public static readonly CocktailType MRedCocktail = new RedCocktailType();
        public static readonly CocktailType MYellowCocktail = new YellowCocktailType();

        private CocktailType(int value, string name) : base(value, name)
        {
        }

        public override ItemType ItemType => ItemType.Cocktail;
        public override GoalType GoalType => GoalType.Cocktail;

        private void CreateCocktail(GameEntity entity, Color color)
        {
            entity.ReplaceLayer(4);
            entity.isTurnSensitive = true;
            entity.isCanBeActivatedByNearMatch = true;
            entity.isRemoverSensitive = true;
            entity.isMultiBlock = true;
            entity.isCantHaveBubbleOnTop = true;

            var colors = new Dictionary<Color, int>
            {
                [color] = 1
            };
            entity.AddColorSensitive(colors);

            var colorArray = new[]
            {
                Color.Blue,
                Color.Green,
                Color.Red,
                Color.Yellow,
                Color.Blue,
                Color.Green,
                Color.Red,
                Color.Yellow,
            };

            var colorQueue = new Queue<Color>();
            var found = false;
            var count = 0;
            foreach (var t in colorArray)
            {
                if (!found && color == t)
                {
                    found = true;
                }

                if (!found)
                {
                    continue;
                }

                if (count >= 4)
                {
                    break;
                }

                colorQueue.Enqueue(t);
                count++;
            }

            entity.AddColorChanging(colorQueue);
            entity.AddColor(color);

            AddFakeItems(entity, entity.willSpawnItem.GridPosition, MultiBlockType.M3X2);
        }

        private class BlueCocktailType : CocktailType
        {
            protected internal BlueCocktailType() : base(185, "BlueCocktail")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCocktail(entity, Color.Blue);
            }
        }

        private class GreenCocktailType : CocktailType
        {
            protected internal GreenCocktailType() : base(182, "GreenCocktail")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCocktail(entity, Color.Green);
            }
        }

        private class RedCocktailType : CocktailType
        {
            protected internal RedCocktailType() : base(183, "RedCocktail")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCocktail(entity, Color.Red);
            }
        }

        private class YellowCocktailType : CocktailType
        {
            protected internal YellowCocktailType() : base(184, "YellowCocktail")
            {
            }

            public override void Spawn(GameEntity entity)
            {
                AddGeneralComponents(entity, ItemType, GoalType);
                CreateCocktail(entity, Color.Yellow);
            }
        }
    }
}