using Microsoft.Extensions.Logging;
using Yag2048.Core.Board;

namespace Yag2048.Game.Board;

public class TileGenerator : ITileGenerator
{
    private readonly Func<ITileCollider, Position, int, ITile> _createTile;
    private readonly Func<int> _createValue;
    private readonly Func<ITileCollider> _getTileCollider;
    private readonly ILogger<TileGenerator> _logger;
    private readonly Random _random;

    public TileGenerator(
        ILogger<TileGenerator> logger,
        Func<ITileCollider> getTileCollider,
        Func<ITileCollider, Position, int, ITile> createTile,
        Func<int> createValue,
        Random random
    )
    {
        _logger = logger;
        _getTileCollider = getTileCollider;
        _createTile = createTile;
        _createValue = createValue;
        _random = random;
    }

    public ITile Generate(ITileGrid grid)
    {
        var freePositions = grid.GetFreePositions().ToList();
        var position = freePositions[_random.Next(freePositions.Count)];
        return _createTile(_getTileCollider(), position, _createValue());
    }

    public ITile GenerateEmpty(Position position) => _createTile(_getTileCollider(), position, default);
}