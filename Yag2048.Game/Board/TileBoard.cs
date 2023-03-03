using Microsoft.Extensions.Logging;
using Yag2048.Core;
using Yag2048.Core.Board;

namespace Yag2048.Game.Board;

public class TileBoard : ITileBoard
{
    private readonly ITileGridMover _tileGridMover;
    private readonly ITileGrid _grid;
    private readonly ILogger<TileBoard> _logger;
    private readonly ITileGenerator _tileGenerator;

    public TileBoard(
        ILogger<TileBoard> logger,
        ITileGrid grid,
        ITileGenerator tileGenerator,
        ITileGridMover tileGridMover
    )
    {
        _logger = logger;
        _grid = grid;
        _tileGenerator = tileGenerator;
        _tileGridMover = tileGridMover;
    }

    public int Width => _grid.Width;

    public int Height => _grid.Height;

    public int MaxTileValue => _grid.MaxTileValue;

    public bool IsMoved { get; private set; }

    public IEnumerable<ITile> GetTiles() => _grid.GetTiles();

    public ITile GetTile(Position position) => !_grid.IsEmpty(position) ? _grid[position] : _tileGenerator.GenerateEmpty(position);

    public ITile GetTile(int row, int col) => GetTile(new Position(row, col));

    public ITileBoard AddRandomTile()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileBoard), nameof(AddRandomTile));

        if (_grid.TilesCount < _grid.Width * _grid.Height)
        {
            var tile = _tileGenerator.Generate(_grid);
            _grid.Add(tile);
        }

        _logger.LogDebug(Utils.FinishMessage0, nameof(TileBoard), nameof(AddRandomTile));

        return this;
    }

    public bool CanMove() => _tileGridMover.CanMove(_grid);

    public ITileBoard Move(MoveDirection direction)
    {
        _logger.LogDebug(Utils.StartMessage1, nameof(TileBoard), nameof(Move), direction);

        IsMoved = _tileGridMover.Move(_grid, direction);

        _logger.LogDebug(Utils.FinishMessage1, nameof(TileBoard), nameof(Move), direction);

        return this;
    }

    public ITileBoard ResetFlags()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileBoard), nameof(ResetFlags));

        _grid.ResetIsMergedFlag();
        IsMoved = false;

        _logger.LogDebug(Utils.FinishMessage0, nameof(TileBoard), nameof(ResetFlags));

        return this;
    }

    public ITileBoard Reset()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(TileBoard), nameof(Reset));

        _grid.Clear();
        AddRandomTile();
        AddRandomTile();

        _logger.LogDebug(Utils.FinishMessage0, nameof(TileBoard), nameof(Reset));

        return this;
    }
}