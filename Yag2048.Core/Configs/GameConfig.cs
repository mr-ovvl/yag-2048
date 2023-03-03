using Yag2048.Core.Game;

namespace Yag2048.Core.Configs;

public class GameConfig
{
    public IDictionary<string, GameAction> KeyMap { get; set; } = GameConfigDefaults.DefaultKeyMap;

    public GameMode GameMode { get; set; } = GameConfigDefaults.DefaultGameMode;

    public IReadOnlyList<ConsoleColor> TileColors { get; set; } = GameConfigDefaults.DefaultTileColors;

    public TileGridConfig GridConfig { get; set; } = null!;
}