using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Color = EntitasBlast.Color;

public class ShuffleSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _matchGroup;
    private int _frameDelay = 1;

    public ShuffleSystem(Contexts contexts)
    {
        _contexts = contexts;
        _matchGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.MatchGroup));
    }

    public void Execute()
    {
        var validMatchCount = 0;
        foreach (var matches in _matchGroup)
        {
            if (matches.matchType.Value == MatchType.Invalid) continue;
            if (matches.matchType.Value == MatchType.Positive) continue;
            if (matches.matchGroup.Count < 2) continue;

            validMatchCount++;
        }

        if (validMatchCount > 0) return;

        if (WaitHelper.Has(
            WaitType.FallingItem,
            WaitType.CriticalAnimation,
            WaitType.WillSpawnItem,
            WaitType.WillBeDestroyedItem,
            WaitType.Activation)) return;

        if (_contexts.game.isCantBeShuffled)
        {
            return;
        }

        if (_frameDelay > 0)
        {
            _frameDelay--;
            return;
        }

        _frameDelay = 1;

        Shuffle();
    }

    private void Shuffle()
    {
        var shuffleList = new List<Tuple<GameEntity, GameEntity>>();

        var cubes = GetColorCubes();
        GetValidCubesAndColors(cubes, out var shuffleCubes, out var colors);
        var shuffleTargetCubes = new List<GameEntity>(shuffleCubes);

        if (!FindFixedMatchCubes(shuffleCubes, out var abreastCubes))
        {
            Debug.LogWarning("No Match Can Be Arranged");
            _contexts.game.isCantBeShuffled = true;
            return;
        }

        _contexts.game.isCantBeShuffled = false;

        GameEntity colorChangedCube = null;
        if (ShouldChangeColor(colors, out var matchColor))
        {
            colorChangedCube = ColorChange(abreastCubes);
            shuffleList.Add(new Tuple<GameEntity, GameEntity>(abreastCubes[0], abreastCubes[1]));
            shuffleList.Add(new Tuple<GameEntity, GameEntity>(abreastCubes[1], abreastCubes[0]));
            shuffleCubes.Remove(abreastCubes[0]);
            shuffleCubes.Remove(abreastCubes[1]);
        }
        else
        {
            var firstPosCube = abreastCubes.FirstOrDefault(c => c.color.Value == matchColor) ?? abreastCubes.First();
            abreastCubes.Remove(firstPosCube);
            abreastCubes.Insert(0, firstPosCube);

            foreach (var posCube in abreastCubes)
            {
                for (var j = shuffleCubes.Count - 1; j >= 0; j--)
                {
                    if (shuffleCubes[j].color.Value != matchColor) continue;
                    if (shuffleCubes[j] == posCube) continue;

                    shuffleList.Add(new Tuple<GameEntity, GameEntity>(shuffleCubes[j], posCube));
                    shuffleCubes.RemoveAt(j);
                    break;
                }
            }
        }

        shuffleTargetCubes.Remove(abreastCubes[0]);
        shuffleTargetCubes.Remove(abreastCubes[1]);

        if (shuffleCubes.Count > 1)
        {
            for (var i = 0; i < shuffleCubes.Count; i++)
            {
                if (shuffleCubes[i].gridPosition.value != shuffleTargetCubes[i].gridPosition.value) continue;

                var next = i > 0 ? -1 : 1;
                var n = shuffleTargetCubes[i];
                shuffleTargetCubes[i] = shuffleTargetCubes[i + next];
                shuffleTargetCubes[i + next] = n;
            }
        }

        for (var i = 0; i < shuffleCubes.Count; i++)
        {
            shuffleList.Add(new Tuple<GameEntity, GameEntity>(shuffleCubes[i], shuffleTargetCubes[i]));
        }

        foreach (var (cube, target) in shuffleList)
        {
            cube.AddShuffleStarted(colorChangedCube != null && cube == colorChangedCube,
                target.gridPosition.value);
            WaitHelper.Increase(WaitType.Fall, WaitType.Hint, WaitType.Input, WaitType.CriticalAnimation);
        }
    }

    private List<GameEntity> GetColorCubes()
    {
        return _contexts.game.GetGroup(GameMatcher.Item).GetEntities()
            .Where(e => Equals(e.itemType.Value, ItemType.ColorCube)).ToList();
    }

    private void GetValidCubesAndColors(List<GameEntity> cubes, out List<GameEntity> shuffleCubes,
        out Dictionary<Color, int> colors)
    {
        var tempColors = new Dictionary<Color, int>();
        var tempShuffleCubes = new List<GameEntity>();

        foreach (var cube in cubes)
        {
            var cellItem =
                _contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(cube.gridPosition.value.x,
                    cube.gridPosition.value.y));

            if (cellItem != null && cellItem.isCanStopItemActivation) continue;

            tempShuffleCubes.Add(cube);

            if (tempColors.ContainsKey(cube.color.Value))
            {
                tempColors[cube.color.Value]++;
            }
            else
            {
                tempColors[cube.color.Value] = 1;
            }
        }

        colors = tempColors;
        shuffleCubes = tempShuffleCubes;
    }

    private bool ShouldChangeColor(Dictionary<Color, int> colors, out Color color)
    {
        foreach (var entry in colors)
        {
            if (entry.Value < 2) continue;

            color = entry.Key;
            return false;
        }

        color = Color.Blue;
        return true;
    }

    private GameEntity ColorChange(List<GameEntity> matchCubes)
    {
        matchCubes.Shuffle();
        var cube1 = matchCubes[0];
        var color = cube1.color.Value;
        var cube2 = matchCubes[1];
        cube2.ReplaceColor(color);
        cube2.ReplaceMatchType(GetMatchType(color));
        cube2.ReplaceGoalType(GetGoalType(color));
        return cube2;
    }

    private bool FindFixedMatchCubes(List<GameEntity> cubes, out List<GameEntity> selectedCubes)
    {
        foreach (var firstCube in cubes.Shuffle())
        {
            var pos = firstCube.gridPosition.value;
            GameEntity secondCube = null;
            var positionsToCheck = new[]
            {
                new Vector2Int(pos.x - 1, pos.y),
                new Vector2Int(pos.x + 1, pos.y),
                new Vector2Int(pos.x, pos.y - 1),
                new Vector2Int(pos.x, pos.y + 1),
            };

            foreach (var checkPos in positionsToCheck)
            {
                var temp = cubes.FirstOrDefault(c => c.gridPosition.value == checkPos);
                if (temp == null) continue;

                secondCube = temp;
                break;
            }

            if (secondCube == null) continue;

            selectedCubes = new List<GameEntity> {firstCube, secondCube};
            return true;
        }

        selectedCubes = null;
        return false;
    }

    private MatchType GetMatchType(Color color)
    {
        switch (color)
        {
            case Color.Blue:
                return MatchType.BlueCube;
            case Color.Green:
                return MatchType.GreenCube;
            case Color.Red:
                return MatchType.RedCube;
            case Color.Yellow:
                return MatchType.YellowCube;
        }

        return MatchType.Invalid;
    }

    private GoalType GetGoalType(Color color)
    {
        switch (color)
        {
            case Color.Blue:
                return GoalType.BlueCube;
            case Color.Green:
                return GoalType.GreenCube;
            case Color.Red:
                return GoalType.RedCube;
            case Color.Yellow:
                return GoalType.YellowCube;
        }

        return GoalType.BlueCube;
    }
}

public class ShuffleEnderSystem : ReactiveSystem<GameEntity>
{
    public ShuffleEnderSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ShuffleEnded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasShuffleEnded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.ReplaceGridPosition(entity.shuffleEnded.Pos);
            entity.ReplacePosition(entity.shuffleEnded.Pos);
            entity.RemoveShuffleStarted();
            entity.RemoveShuffleEnded();
            WaitHelper.Reduce(WaitType.Fall, WaitType.Hint, WaitType.Input, WaitType.CriticalAnimation);
        }
    }
}