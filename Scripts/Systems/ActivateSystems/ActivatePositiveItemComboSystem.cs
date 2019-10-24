using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using EnumeratorExtensions;
using UnityEngine;
using Color = EntitasBlast.Color;

public class ActivatePositiveItemComboSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    private readonly IGroup<GameEntity> _itemGroup;

    public ActivatePositiveItemComboSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _itemGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.MatchGroup, GameMatcher.Item)
            .NoneOf(GameMatcher.Falling, GameMatcher.WillBeDestroyed));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Activated);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isActivated && entity.hasPositiveItemMatch;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var id = entity.positiveItemMatch.TouchedPositiveItemId;
            var touchedPosItem = _contexts.game.GetEntityWithId(id);
            ActivatePositiveItemMatch(touchedPosItem).Start();
            entity.isWillBeDestroyed = true;
        }
    }

    private IEnumerator ActivatePositiveItemMatch(GameEntity touchedItem)
    {
        WaitHelper.Increase(WaitType.Input, WaitType.CriticalAnimation);

        TryDestroyBubbleOnTop(touchedItem);

        touchedItem.isMergeMaster = true;

        var pos = touchedItem.gridPosition.value;
        var matchId = touchedItem.matchGroup.Id;
        var posItems = _contexts.game.GetEntitiesWithMatchGroup(matchId);

        var rotorCount = 0;
        var tntCount = 0;
        var puzzleCount = 0;
        var puzzleColor = Color.Blue;

        var posItemIds = new List<int>();
        foreach (var posItem in posItems)
        {
            if (Equals(posItem.itemType.Value, ItemType.Rotor))
            {
                rotorCount++;
            }

            if (Equals(posItem.itemType.Value, ItemType.Tnt))
            {
                tntCount++;
            }

            if (Equals(posItem.itemType.Value, ItemType.Puzzle))
            {
                puzzleColor = posItem.color.Value;
                puzzleCount++;
            }

            posItemIds.Add(posItem.id.Value);
            posItem.AddMerging(touchedItem.gridPosition.value);
            posItem.isCanFall = false;
            posItem.isCantBeActivated = true;
            TryDestroyBubbleOnTop(posItem);
        }

        yield return DoWait.WaitWhile(() => touchedItem.isMergeMaster);

        foreach (var id in posItemIds)
        {
            var posItem = _contexts.game.GetEntityWithId(id);
            posItem.isWillBeDestroyed = true;
        }

        CreateCombo(pos, rotorCount, tntCount, puzzleCount, puzzleColor);
        WaitHelper.Reduce(WaitType.Input, WaitType.CriticalAnimation);
    }

    private void TryDestroyBubbleOnTop(GameEntity puzzle)
    {
        var cellItem =
            _contexts.game.GetEntityWithCellItemId(new Tuple<int, int>(puzzle.gridPosition.value.x,
                puzzle.gridPosition.value.y));

        if (cellItem == null) return;
        if (!cellItem.isCanBeActivatedByInnerMatch) return;

        cellItem.isWillBeDestroyed = true;
    }

    private void CreateCombo(Vector2Int pos, int rotorCount, int tntCount, int puzzleCount, Color puzzleColor)
    {
        var comboEntity = _contexts.game.CreateEntity();
        comboEntity.isCreatedFromMatch = true;

        if (puzzleCount > 0)
        {
            if (puzzleCount > 1)
            {
                CreatePuzzlePuzzleCombo(comboEntity, pos);
            }
            else if (tntCount > 0)
            {
                CreatePuzzleTntCombo(comboEntity, puzzleColor, pos);
            }
            else if (rotorCount > 0)
            {
                CreatePuzzleRotorCombo(comboEntity, puzzleColor, pos);
            }
        }
        else if (tntCount > 0)
        {
            if (tntCount > 1)
            {
                CreateTntTntCombo(comboEntity, pos);
            }
            else if (rotorCount > 0)
            {
                CreateTntRotorCombo(comboEntity, pos);
            }
        }
        else if (rotorCount > 1)
        {
            CreateRotorRotorCombo(comboEntity, pos);
        }
    }

    private static void CreatePuzzlePuzzleCombo(GameEntity puzzlePuzzle, Vector2Int pos)
    {
        puzzlePuzzle.AddGridPosition(pos);
        puzzlePuzzle.isPuzzlePuzzle = true;
        puzzlePuzzle.isActivated = true;
        puzzlePuzzle.AddComboType(ComboType.PuzzlePuzzle);
    }

    private static void CreatePuzzleTntCombo(GameEntity puzzleCombo, Color puzzleColor, Vector2Int pos)
    {
        puzzleCombo.AddPuzzleCombo(PuzzleComboType.PuzzleTnt);
        puzzleCombo.AddGridPosition(pos);
        puzzleCombo.AddColor(puzzleColor);
        puzzleCombo.isActivated = true;
        puzzleCombo.AddComboType(ComboType.PuzzleCombo);
    }

    private static void CreatePuzzleRotorCombo(GameEntity puzzleCombo, Color puzzleColor, Vector2Int pos)
    {
        puzzleCombo.AddPuzzleCombo(PuzzleComboType.PuzzleRotor);
        puzzleCombo.AddGridPosition(pos);
        puzzleCombo.AddColor(puzzleColor);
        puzzleCombo.isActivated = true;
        puzzleCombo.AddComboType(ComboType.PuzzleCombo);
    }

    private static void CreateTntTntCombo(GameEntity tntTnt, Vector2Int pos)
    {
        tntTnt.AddTntTnt(3);
        tntTnt.AddGridPosition(pos);
        tntTnt.isActivated = true;
        tntTnt.AddComboType(ComboType.TntTnt);
    }

    private static void CreateRotorRotorCombo(GameEntity rotorRotor, Vector2Int pos)
    {
        rotorRotor.AddGridPosition(pos);
        rotorRotor.isRotorRotor = true;
        rotorRotor.isActivated = true;
        rotorRotor.AddComboType(ComboType.RotorRotor);
    }

    private static void CreateTntRotorCombo(GameEntity tntRotor, Vector2Int pos)
    {
        tntRotor.AddGridPosition(pos);
        tntRotor.isTntRotor = true;
        tntRotor.isActivated = true;
        tntRotor.AddComboType(ComboType.TntRotor);
    }
}