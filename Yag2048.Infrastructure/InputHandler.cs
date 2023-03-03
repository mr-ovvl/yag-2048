using Microsoft.Extensions.Options;
using Yag2048.Core.Configs;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure;

public class InputHandler : IInputHandler
{
    private readonly IOptionsMonitor<GameConfig> _config;

    public InputHandler(IOptionsMonitor<GameConfig> config)
    {
        _config = config;
    }

    public GameAction GetAction()
    {
        var key = Console.ReadKey();
        if (_config.CurrentValue.KeyMap.TryGetValue(key.Key.ToString(), out var gameAction))
            return gameAction;
        return GameAction.Unknown;
    }
}