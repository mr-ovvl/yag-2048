using Yag2048.Core.Stats;

namespace Yag2048.Stats;

public class TileStats : ITileStats
{
    internal TileStats(int value, long movesCount, TimeSpan elapsedTime)
    {
        Value = value;
        MovesCount = movesCount;
        ElapsedTime = elapsedTime;
    }

    public int Value { get; }

    public long MovesCount { get; private set; }

    public TimeSpan ElapsedTime { get; private set; }

    public void Update(long movesCount, TimeSpan elapsedTime)
    {
        MovesCount = movesCount;
        ElapsedTime = elapsedTime;
    }

    public static TileStats Create(int value, long movesCount, TimeSpan elapsedTime) => new (value, movesCount, elapsedTime);
}