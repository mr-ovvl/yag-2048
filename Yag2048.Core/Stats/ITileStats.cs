namespace Yag2048.Core.Stats;

public interface ITileStats
{
    int Value { get; }

    long MovesCount { get; }

    TimeSpan ElapsedTime { get; }

    void Update(long movesCount, TimeSpan elapsedTime);
}
