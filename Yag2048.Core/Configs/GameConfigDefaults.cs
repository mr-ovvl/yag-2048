using System.Collections.Immutable;
using Yag2048.Core.Game;

namespace Yag2048.Core.Configs;

public static class GameConfigDefaults
{
    public static ImmutableDictionary<string, GameAction> DefaultKeyMap = new Dictionary<string, GameAction>
    {
        { "W", GameAction.MoveUp },
        { "S", GameAction.MoveDown },
        { "A", GameAction.MoveLeft },
        { "D", GameAction.MoveRight },
        { "E", GameAction.ChangeMode },
        { "R", GameAction.StartGame },
        { "Y", GameAction.Confirm },
        { "N", GameAction.Cancel },
        { "P", GameAction.ShowSettings },
        { "Q", GameAction.QuitGame }
    }.ToImmutableDictionary();

    public static ImmutableArray<ConsoleColor> DefaultTileColors = ImmutableArray.Create(
        ConsoleColor.Black,
        ConsoleColor.DarkRed,
        ConsoleColor.DarkYellow,
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkBlue,
        ConsoleColor.DarkMagenta,
        ConsoleColor.DarkGray,
        ConsoleColor.Red,
        ConsoleColor.Yellow,
        ConsoleColor.Green,
        ConsoleColor.Cyan,
        ConsoleColor.Blue
    );

    public static GameMode DefaultGameMode = GameMode.Game2048;
}