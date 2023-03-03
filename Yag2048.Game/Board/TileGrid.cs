using Microsoft.Extensions.Logging;
using Yag2048.Core;
using Yag2048.Core.Board;
using Yag2048.Core.Configs;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Game.Board;

public class TileGrid : ITileGrid
{
    private readonly Func<TileSliceType, IEnumerable<ITile>, ITileSlice> _createSlice;

    private readonly ILogger<TileGrid> _logger;

    private readonly IWritableOptions<TileGridConfig> _options;

    private readonly IDictionary<Position, ITile> _tiles = new Dictionary<Position, ITile>();

    public TileGrid(
        ILogger<TileGrid> logger,
        IWritableOptions<TileGridConfig> options,
        Func<TileSliceType, IEnumerable<ITile>, ITileSlice> createSlice
    )
    {
        _logger = logger;
        _options = options;
        _createSlice = createSlice;

        Width = _options.CurrentValue.Width;
        Height = _options.CurrentValue.Height;
    }

    public int Width { get; init; }

    public int Height { get; init; }

    public int TilesCount => _tiles.Count;

    public int MaxTileValue { get; private set; } = default;

    public ITile this[Position position] => _tiles[position];

    public ITileGrid Clear()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileGrid), nameof(ResetIsMergedFlag));

        _tiles.Clear();
        MaxTileValue = default;

        _logger.LogDebug(Utils.FinishMessage0, nameof(TileGrid), nameof(ResetIsMergedFlag));

        return this;
    }

    public bool IsFull()
    {
        return _tiles.Count == Width * Height;
    }

    public bool IsEmpty()
    {
        return !_tiles.Any();
    }

    public bool IsEmpty(Position position)
    {
        return !_tiles.ContainsKey(position);
    }

    public bool IsEmpty(int row, int col)
    {
        return IsEmpty(new Position(row, col));
    }

    public bool IsWithin(Position position)
    {
        return position.Row >= 0
               && position.Row < Height
               && position.Col >= 0
               && position.Col < Width;
    }

    public IEnumerable<ITile> GetTiles() => _tiles.Values;

    public IEnumerable<ITile> GetRow(int row) => _tiles.Where(x => x.Key.Row == row).Select(x => x.Value).OrderBy(x => x.Position.Col);

    public IEnumerable<ITile> GetCol(int col) => _tiles.Where(x => x.Key.Col == col).Select(x => x.Value).OrderBy(x => x.Position.Row);

    public ITileSlice GetSlice(TileSliceType type, int index) =>
        type switch
        {
            TileSliceType.Horizontal => _createSlice(type, GetRow(index).ToList()),
            TileSliceType.Vertical => _createSlice(type, GetCol(index).ToList()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    public ITileGrid Add(ITile tile)
    {
        _logger.LogDebug(Utils.StartMessage1, nameof(TileGrid), nameof(Add), tile);

        _tiles.Add(tile.Position, tile);
        if (tile.Value > MaxTileValue)
        {
            MaxTileValue = tile.Value;
        }

        _logger.LogDebug(Utils.FinishMessage1, nameof(TileGrid), nameof(Add), tile);

        return this;
    }

    public ITileGrid Remove(Position position)
    {
        _logger.LogDebug(Utils.StartMessage1, nameof(TileGrid), nameof(Remove), position);
        if (!_tiles.ContainsKey(position))
            throw new ArgumentException(nameof(position));

        _tiles.Remove(position);
        _logger.LogDebug(Utils.FinishMessage1, nameof(TileGrid), nameof(Remove), position);

        return this;
    }

    public void ResetIsMergedFlag()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileGrid), nameof(ResetIsMergedFlag));

        foreach (var tile in _tiles)
            tile.Value.IsMerged = false;

        _logger.LogDebug(Utils.FinishMessage0, nameof(TileGrid), nameof(ResetIsMergedFlag));
    }

    public ITileGrid UpdatePosition(Position oldPosition, Position newPosition)
    {
        _logger.LogDebug(Utils.StartMessage2, nameof(TileGrid), nameof(UpdatePosition), oldPosition, newPosition);

        if (!_tiles.ContainsKey(oldPosition))
            throw new ArgumentException(nameof(oldPosition));

        _tiles[newPosition] = _tiles[oldPosition];
        if (_tiles[newPosition].Value > MaxTileValue)
        {
            MaxTileValue = _tiles[newPosition].Value;
        }

        _logger.LogDebug(Utils.FinishMessage2, nameof(TileGrid), nameof(UpdatePosition), oldPosition, newPosition);

        return Remove(oldPosition);
    }

    public IEnumerable<Position> GetFreePositions()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileGrid), nameof(GetFreePositions));

        var isEmpty = IsEmpty();
        for (var row = 0; row < Height; row++)
        {
            for (var col = 0; col < Width; col++)
            {
                var pos = new Position(row, col);
                if (isEmpty || IsEmpty(pos))
                    yield return pos;
            }
        }
    }
}