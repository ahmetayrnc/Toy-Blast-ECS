using System.Collections.Generic;
using Entitas;

public class CellStateDeterminationSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;

    private CellComponent[] _cells;
    private GameEntity[] _cellEntities;
    private bool[] _itemPositions;
    private bool[] _fallStoppingCellItems;
    private bool[] _activeStoppingCellItems;

    private readonly IGroup<GameEntity> _cellGroup;
    private readonly IGroup<GameEntity> _itemGroup;
    private readonly IGroup<GameEntity> _cellItemGroup;

    private int _width;
    private int _height;

    public CellStateDeterminationSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.Cell);
        _itemGroup = contexts.game.GetGroup(GameMatcher.AnyOf(GameMatcher.Item, GameMatcher.FakeItem));
        _cellItemGroup = contexts.game.GetGroup(GameMatcher.CellItem);
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cells = new CellComponent[_width * _height];
        _cellEntities = new GameEntity[_width * _height];
        _itemPositions = new bool[_width * _height];
        _fallStoppingCellItems = new bool[_width * _height];
        _activeStoppingCellItems = new bool[_width * _height];
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.CellsDirty));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCellsDirty;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        DetermineCellStates();
    }

    private void DetermineCellStates()
    {
        _contexts.game.isCellsDirty = false;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var key = _width * y + x;
                _itemPositions[key] = false;
                _fallStoppingCellItems[key] = false;
                _activeStoppingCellItems[key] = false;
            }
        }

        foreach (var e in _cellGroup)
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _cells[key] = e.cell;
            _cellEntities[key] = e;
        }

        foreach (var item in _itemGroup)
        {
            var pos = item.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _itemPositions[key] = true;
        }

        foreach (var e in _cellItemGroup)
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            if (e.isCanStopItemFall)
                _fallStoppingCellItems[key] = true;
            if (e.isCanStopItemActivation)
                _activeStoppingCellItems[key] = true;
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var key = _width * y + x;

                if (_cellEntities[key].cellState.Value == CellState.Invalid)
                {
                    continue;
                }

                CellState cellState = CellState.Empty;
                bool canAcceptFall = true;
                bool canLetItemFall = true;
                bool canLetItemMatch = true;

                if (_itemPositions[key])
                {
                    cellState = CellState.Full;
                    canAcceptFall = false;
                }

                if (_fallStoppingCellItems[key])
                {
                    canLetItemFall = false;
                }

                if (_activeStoppingCellItems[key])
                {
                    canLetItemMatch = false;
                }

                if (_cellEntities[key].canAcceptFall.Counter > 0)
                {
                    canAcceptFall = false;
                }

                _cellEntities[key].ReplaceCellState(cellState);
                _cellEntities[key].ReplaceCanAcceptFall(canAcceptFall, _cellEntities[key].canAcceptFall.Counter);
                _cellEntities[key].ReplaceCell(canLetItemMatch, canLetItemFall);
            }
        }
    }
}