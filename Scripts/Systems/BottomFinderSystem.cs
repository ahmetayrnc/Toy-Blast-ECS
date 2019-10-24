using Entitas;

public sealed class BottomFinderSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public BottomFinderSystem(Contexts contexts)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.Cell);
    }

    private GameEntity[,] _cellEntities;

    private readonly IGroup<GameEntity> _cellGroup;
    private int _width;
    private int _height;

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cellEntities = new GameEntity[_width, _height];

        foreach (var e in _cellGroup)
        {
            var pos = e.gridPosition.value;
            _cellEntities[pos.x, pos.y] = e;
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_cellEntities[x, y].cellState.Value == CellState.Invalid) continue;

                _cellEntities[x, y].isBottomCell = true;
                break;
            }
        }
    }
}