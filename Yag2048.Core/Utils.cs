using Yag2048.Core.Board;
using Yag2048.Core.Game;

namespace Yag2048.Core;

public static class Utils
{
    public const string StartMessage0 = "{class}.{method} started.";
    public const string FinishMessage0 = "{class}.{method} finished.";
    public const string StartMessage1 = "{class}.{method} with arg: [{arg}] started.";
    public const string FinishMessage1 = "{class}.{method} with arg: [{arg}] finished.";
    public const string StartMessage2 = "{class}.{method} with arg: [{arg1}, {arg2}] started.";
    public const string FinishMessage2 = "{class}.{method} with arg: [{arg}, {arg2}] finished.";

    public static void CheckRange(int value, int minValue, int maxValue, string paramName)
    {
        if (minValue > maxValue)
            throw new ArgumentException($"{nameof(minValue)} should be less of {nameof(maxValue)}.");

        if (value < minValue || value > maxValue)
            throw new ArgumentOutOfRangeException(paramName, value, $"Value should be between {minValue} and {maxValue}.");
    }

    public static TileSliceType GetSliceType(MoveDirection direction)
    {
        return direction switch
        {
            MoveDirection.Up => TileSliceType.Horizontal,
            MoveDirection.Down => TileSliceType.Horizontal,
            MoveDirection.Left => TileSliceType.Vertical,
            MoveDirection.Right => TileSliceType.Vertical,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static int GetStartSliceIndex(ITileGrid grid, MoveDirection direction)
    {
        return direction switch
        {
            MoveDirection.Up => 0,
            MoveDirection.Down => grid.Height - 1,
            MoveDirection.Left => 0,
            MoveDirection.Right => grid.Width - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static int GetEndSliceIndex(ITileGrid grid, MoveDirection direction)
    {
        return direction switch
        {
            MoveDirection.Up => grid.Height - 1,
            MoveDirection.Down => 0,
            MoveDirection.Left => grid.Width - 1,
            MoveDirection.Right => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static void Initialize(IGameContext gameContext)
    {
        gameContext.Init();
        if (gameContext.GameStatus == GameStatus.Initialized)
        {
            gameContext.GameStatus = GameStatus.Running;
        }
        else
        {
            throw new ApplicationException("GameContext is not initialized.");
        }
    }
}