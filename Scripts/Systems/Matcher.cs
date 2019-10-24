using System.Collections.Generic;
using Entitas;

public class Matcher : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;

    private MatchType[] _matchTypes;
    private CellComponent[] _cells;

    private int[] _matchIds;
    private int[] _matchCounts;
    private bool[] _hasFallings;
    private bool[] _fallings;

    private readonly IGroup<GameEntity> _cellGroup;
    private readonly IGroup<GameEntity> _itemGroup;

    private int _width;
    private int _height;

    public Matcher(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _cellGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Cell));
        _itemGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.MatchType)
            .NoneOf(GameMatcher.WillBeDestroyed, GameMatcher.Merging));
    }

    public void Initialize()
    {
        var boardSize = _contexts.game.board.Size;
        _width = boardSize.x;
        _height = boardSize.y;
        _cells = new CellComponent[_width * _height];
        _matchTypes = new MatchType[_width * _height];
        _matchIds = new int[_width * _height];
        _matchCounts = new int[_width * _height + 1];
        _hasFallings = new bool[_width * _height + 1];
        _fallings = new bool[_width * _height + 1];
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Dirty);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDirty;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.game.isDirty = false;

        if (WaitHelper.Has(WaitType.Hint)) return;

        var currentMatchGroupId = 0;

        //reset id and type
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var key = _width * y + x;
                _matchIds[key] = -1;
                _matchTypes[key] = MatchType.Invalid;
                _matchCounts[key] = -1;
                _hasFallings[key] = false;
            }
        }

        foreach (var e in _cellGroup)
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _cells[key] = e.cell;
        }

        foreach (var e in _itemGroup)
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            _matchTypes[key] = e.matchType.Value;
            _fallings[key] = e.hasFalling;
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var key = _width * y + x;

                var currentType = _matchTypes[key];

                if (currentType == MatchType.Invalid)
                {
                    continue;
                }

                _matchCounts[currentMatchGroupId] = 0;
                var gotFilled = MyFill(currentType, currentMatchGroupId, x, y, _width, _height);
                if (gotFilled)
                {
                    currentMatchGroupId++;
                }

                if (_fallings[key])
                {
                    _hasFallings[currentMatchGroupId] = true;
                }
            }
        }

        foreach (var e in _itemGroup.GetEntities())
        {
            var pos = e.gridPosition.value;
            var key = _width * pos.y + pos.x;
            var matchId = _matchIds[key];
            var count = matchId == -1 ? 0 : _matchCounts[matchId];

            var hintType = HintType.None;
            if (count >= 9)
            {
                hintType = HintType.Puzzle;
            }
            else if (count >= 7)
            {
                hintType = HintType.Tnt;
            }
            else if (count >= 5)
            {
                hintType = HintType.Rotor;
            }
            else if (count >= 2)
            {
                if (e.isPositiveItem)
                {
                    hintType = HintType.Positive;
                }
            }

            e.ReplaceMatchGroup(matchId, count);

            if (_hasFallings[currentMatchGroupId])
            {
                continue;
            }

            if (!e.hasHint)
            {
                continue;
            }

            if (e.hint.Value == hintType)
            {
                continue;
            }

            e.ReplaceHint(hintType);
        }
    }

    private bool CanAddToGroup(MatchType currentType, int x, int y)
    {
        var key = _width * y + x;

        if (_matchTypes[key] == MatchType.Invalid)
            return false;

        if (_matchTypes[key] != currentType)
            return false;

        if (_matchIds[key] != -1)
            return false;

        if (!_cells[key].CanLetItemActivate)
            return false;

        return true;
    }

    private bool MyFill(MatchType currentType, int matchId, int x, int y, int width, int height)
    {
        if (CanAddToGroup(currentType, x, y))
        {
            _MyFill(currentType, matchId, x, y, width, height);
            return true;
        }

        return false;
    }

    private void _MyFill(MatchType currentType, int matchId, int x, int y, int width, int height)
    {
        while (true)
        {
            int ox = x, oy = y;
            while (y != 0 && CanAddToGroup(currentType, x, y - 1)) y--;
            while (x != 0 && CanAddToGroup(currentType, x - 1, y)) x--;
            if (x == ox && y == oy) break;
        }

        MyFillCore(currentType, matchId, x, y, width, height);
    }

    private void MyFillCore(MatchType currentType, int matchId, int x, int y, int width, int height)
    {
        int lastRowLength = 0;
        do
        {
            int rowLength = 0, sx = x;
            if (lastRowLength != 0 && !CanAddToGroup(currentType, x, y))
            {
                do
                {
                    if (--lastRowLength == 0) return;
                } while (!CanAddToGroup(currentType, ++x, y));

                sx = x;
            }
            else
            {
                for (; x != 0 && CanAddToGroup(currentType, x - 1, y); rowLength++, lastRowLength++)
                {
                    var key = _width * y + --x;
                    _matchIds[key] = matchId;
                    _matchCounts[matchId] += 1;
                    if (y != 0 && CanAddToGroup(currentType, x, y - 1))
                        _MyFill(currentType, matchId, x, y - 1, width, height);
                }
            }

            for (; sx < width && CanAddToGroup(currentType, sx, y); rowLength++, sx++)
            {
                var key = _width * y + sx;
                _matchIds[key] = matchId;
                _matchCounts[matchId] += 1;
            }

            if (rowLength < lastRowLength)
            {
                for (int end = x + lastRowLength; ++sx < end;)
                {
                    if (CanAddToGroup(currentType, sx, y)) MyFillCore(currentType, matchId, sx, y, width, height);
                }
            }
            else if (rowLength > lastRowLength && y != 0)
            {
                for (int ux = x + lastRowLength; ++ux < sx;)
                {
                    if (CanAddToGroup(currentType, ux, y - 1))
                        _MyFill(currentType, matchId, ux, y - 1, width, height);
                }
            }

            lastRowLength = rowLength;
        } while (lastRowLength != 0 && ++y < height);
    }
}