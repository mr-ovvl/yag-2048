using Microsoft.Extensions.Options;
using Yag2048.Core.Board;
using Yag2048.Core.Configs;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;
using Yag2048.Core.Stats;

namespace Yag2048.Infrastructure;

public class SimpleConsoleRenderer : IRenderer
{
    private const int _horizontalBorderOffset = 5;
    private const int _verticalalBorderOffset = 2;
    private const int _width = 128;
    private const int _height = 24;

    private const int _itemVerticalOffset = 1;

    private const string _gameTitle = """
             ___  _    _  _   ___  
            |__ \| |  | || | / _ \ 
               ) | | _| || || (_) |
              / /| |/ /__   _> _ < 
             / /_|   <   | || (_) |
            |____|_|\_\  |_| \___/ 
        """;

    private const byte _gameTitleHeight = 7;

    private const byte _gameModeHeight = 1;

    private const byte _leftMenuWidth = 23;

    private const byte _boardMargin = 4;

    private const byte _boardOffset = _leftMenuWidth + _boardMargin;

    private const byte _tileHeight = 5;

    private const byte _tileWidth = 10;

    private readonly string _emptyLine;

    private readonly IOptionsMonitor<GameConfig> _gameConfig;

    private bool _isTitleRendered;

    public SimpleConsoleRenderer(IOptionsMonitor<GameConfig> gameConfig)
    {
        _gameConfig = gameConfig;
        _emptyLine = string.Intern(new string(' ', _width));
    }

    public void Render(IGameContext context)
    {
        if (OperatingSystem.IsWindows())
        {
            Console.SetWindowSize(106, 28);
        }

        Clear();

        if (!_isTitleRendered)
            RenderTitle();

        RenderMode(context.Mode);
        RenderMenuActions();
        RenderStats(context.Stats, context.TileBoard.Width);

        for (var row = 0; row < context.TileBoard.Height; row++)
        {
            for (var col = 0; col < context.TileBoard.Width; col++)
            {
                var tile = context.TileBoard.GetTile(row, col);
                RenderTile(tile);
            }
        }

        if (context.GameStatus == GameStatus.Win)
        {
            RenderWin();
        }

        if (context.GameStatus == GameStatus.Lose)
        {
            RenderLose();
        }
    }

    private void Clear()
    {
        for (var i = 0; i <= _height; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(_emptyLine);
        }

        _isTitleRendered = false;
    }

    private void RenderTile(ITile tile)
    {
        const int tileInnerWidth = _tileWidth - 2;
        var emptyStrValue = string.Intern(new string(' ', tileInnerWidth));
        var index = tile.Value == 0 ? 0 : ((int)Math.Log2(tile.Value)) % _gameConfig.CurrentValue.TileColors.Count;
        Console.BackgroundColor = _gameConfig.CurrentValue.TileColors[index];
        var strValue = tile.Value == default ? emptyStrValue : GetPaddedStrValue(tile.Value, tileInnerWidth, ' ');
        var leftOffset = _boardOffset + (tile.Position.Col * _tileWidth);
        var topOffset = tile.Position.Row * _tileHeight;
        var rowNum = 0;
        WriteLine("┌────────┐", leftOffset, topOffset, ref rowNum);
        WriteLine("│        │", leftOffset, topOffset, ref rowNum);
        WriteLine($"│{strValue}│", leftOffset, topOffset, ref rowNum);
        WriteLine("│        │", leftOffset, topOffset, ref rowNum);
        WriteLine("└────────┘", leftOffset, topOffset, ref rowNum);
        Console.ResetColor();
    }

    private void RenderTitle()
    {
        Console.SetCursorPosition(0, _verticalalBorderOffset - 1);
        Console.Write(_gameTitle);
        Console.SetCursorPosition(_horizontalBorderOffset, _verticalalBorderOffset + _gameTitleHeight - 1);
        Console.Write("~Yet another 2048 game~");
        _isTitleRendered = true;
    }

    private void RenderMenuActions()
    {
        const int leftOffset = 0;
        const int topOffset = _gameTitleHeight + +_gameModeHeight + _itemVerticalOffset * 2;
        var rowNum = 0;
        WriteLine("Actions:", leftOffset, topOffset, ref rowNum);
        WriteLine($"Move ↑|↓|←|→:  {string.Join('|', GetMoveButtons()),8}", leftOffset, topOffset, ref rowNum);
        WriteLine($"Start new game:  {GetButton(GameAction.StartGame),6}", leftOffset, topOffset, ref rowNum);
        WriteLine($"Change mode: {GetButton(GameAction.ChangeMode),10}", leftOffset, topOffset, ref rowNum);
        WriteLine($"Quit:  {GetButton(GameAction.QuitGame),16}", leftOffset, topOffset, ref rowNum);

        string GetButton(GameAction gameAction) => _gameConfig.CurrentValue.KeyMap.First(x => x.Value == gameAction).Key;
        string[] GetMoveButtons() =>
            _gameConfig.CurrentValue.KeyMap.Where(x =>
                x.Value == GameAction.MoveUp
                || x.Value == GameAction.MoveDown
                || x.Value == GameAction.MoveLeft
                || x.Value == GameAction.MoveRight)
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .ToArray();
    }

    private static void RenderMode(GameMode mode)
    {
        Console.SetCursorPosition(_horizontalBorderOffset, _verticalalBorderOffset + _gameTitleHeight + _itemVerticalOffset);
        Console.Write($"Mode:        {mode,10}");
    }

    private static void RenderStats(IStats stats, int boardWidth)
    {
        int leftOffset = _boardOffset + _tileWidth * boardWidth + _boardMargin;
        const int topOffset = -1;
        var rowNum = 0;
        WriteLine("-------- Stats --------", leftOffset, topOffset, ref rowNum);
        WriteLine($"Score:       {stats.Score,10}", leftOffset, topOffset, ref rowNum);
        WriteLine($"Moves:       {stats.MovesCount,10}", leftOffset, topOffset, ref rowNum);
        WriteLine($"BestTile:    {stats.BestTileStats.Value,10}", leftOffset, topOffset, ref rowNum);
        WriteLine($"Elapsed:    {stats.ElapsedTime.Seconds,10}s", leftOffset, topOffset, ref rowNum);

        var keys = stats.TilesStats.Keys.ToList();
        WriteLine("----- Top 5 Tiles -----", leftOffset, topOffset, ref rowNum);
        WriteTileStats(keys.Count - 1);
        WriteTileStats(keys.Count - 2);
        WriteTileStats(keys.Count - 3);
        WriteTileStats(keys.Count - 4);
        WriteTileStats(keys.Count - 5);

        void WriteTileStats(int index)
        {
            if (index > 0 && index < keys!.Count)
            {
                var tileStats = stats.TilesStats[keys![index]];
                WriteLine(GetPaddedStrValue(tileStats.Value, _leftMenuWidth, '-'), leftOffset, topOffset, ref rowNum);
                WriteLine($"Moves:       {tileStats.MovesCount,10}", leftOffset, topOffset, ref rowNum);
                WriteLine($"Elapsed:    {tileStats.ElapsedTime.Seconds,10}s", leftOffset, topOffset, ref rowNum);
            }
        }
    }

    private static void WriteLine(string str, int leftOffset, int topOffset, ref int rowNum)
    {
        Console.SetCursorPosition(_horizontalBorderOffset + leftOffset, _verticalalBorderOffset + topOffset + rowNum);
        Console.WriteLine(str);
        rowNum++;
    }

    private static void RenderWin()
    {
        const int leftOffset = _horizontalBorderOffset + 32;
        const int topOffset = _verticalalBorderOffset + 4;
        int rowNum = 0;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine("╔══════════════════╗", leftOffset, topOffset, ref rowNum);
        WriteLine("║                  ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║     You win!     ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║ Continue?  (Y/N) ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║                  ║", leftOffset, topOffset, ref rowNum);
        WriteLine("╚══════════════════╝", leftOffset, topOffset, ref rowNum);
        Console.ResetColor();
    }

    private static void RenderLose()
    {
        const int leftOffset = _horizontalBorderOffset + 32;
        const int topOffset = _verticalalBorderOffset + 4;
        int rowNum = 0;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine("╔══════════════════╗", leftOffset, topOffset, ref rowNum);
        WriteLine("║                  ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║    Game over!    ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║  Restart? (Y/N)  ║", leftOffset, topOffset, ref rowNum);
        WriteLine("║                  ║", leftOffset, topOffset, ref rowNum);
        WriteLine("╚══════════════════╝", leftOffset, topOffset, ref rowNum);
        Console.ResetColor();
    }

    private static string GetPaddedStrValue(int value, byte width, char paddingChar)
    {
        var valueLength = (int)Math.Floor(Math.Log10(value) + 1);
        var padRight = (width - valueLength) / 2;
        return value.ToString()
            .PadRight(padRight, paddingChar)
            .PadLeft(width, paddingChar);
    }
}