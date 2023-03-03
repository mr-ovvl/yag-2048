using Yag2048.Core.Stats;

namespace Yag2048.Stats;

public class LocalStats : IStats
{
    private readonly Func<int, long, TimeSpan, ITileStats> _createTileStats;

    private readonly SortedDictionary<int, ITileStats> _tilesStats = new ();

    public LocalStats(Func<int, long, TimeSpan, ITileStats> createTileStats)
    {
        _createTileStats = createTileStats;
        BestTileStats = _createTileStats(0, 0, TimeSpan.Zero);
    }

    public long Score { get; private set; }

    public long MovesCount { get; private set; }

    public TimeSpan ElapsedTime { get; private set; }

    public ITileStats BestTileStats { get; private set; }

    public IReadOnlyDictionary<int, ITileStats> TilesStats => _tilesStats;

    public void Update(long score, int maxTileValue, TimeSpan elapsedTime)
    {
        if (MovesCount < long.MaxValue)
            MovesCount++;

        if (Score < score)
            Score = score;

        if (ElapsedTime < elapsedTime)
            ElapsedTime = elapsedTime;

        if (BestTileStats.Value < maxTileValue)
        {
            BestTileStats = _createTileStats(maxTileValue, MovesCount, ElapsedTime);
            if (!_tilesStats.ContainsKey(BestTileStats.Value)
                || (_tilesStats.TryGetValue(BestTileStats.Value, out var value) && (value.MovesCount < BestTileStats.MovesCount)))
            {
                _tilesStats[BestTileStats.Value] = BestTileStats;
            }
        }
    }
}