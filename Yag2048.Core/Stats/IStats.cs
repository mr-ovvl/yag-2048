namespace Yag2048.Core.Stats;

public interface IStats
{
    long Score { get; }

    long MovesCount { get; }

    TimeSpan ElapsedTime { get; }

    ITileStats BestTileStats { get; }

    IReadOnlyDictionary<int, ITileStats> TilesStats { get; }

    void Update(long score, int maxTileValue, TimeSpan elapsedTime);
}
