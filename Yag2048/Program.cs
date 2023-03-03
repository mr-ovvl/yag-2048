using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Yag2048;
using Yag2048.Core.Configs;
using Yag2048.Game;
using Yag2048.Game.Rules2048;
using Yag2048.Game.RulesThrees;
using Yag2048.Infrastructure;
using Yag2048.Stats;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.File("yag2048.log")
    .CreateLogger();

Log.Debug("test");

await Host.CreateDefaultBuilder()
    .ConfigureLogging(logging => logging.AddSerilog())
    .ConfigureServices((context, services) =>
    {
        services.ConfigureWritable<GameConfig>(context.Configuration.GetSection("AppConfig"));
        services.ConfigureWritable<TileGridConfig>(context.Configuration.GetSection("AppConfig:GridConfig"));

        services
            .AddTileValueFactory()
            .AddTileColliderProvider()
            .AddTileSliceMoverProvider()
            .AddGameRulesProvider()
            .AddGame()
            .AddGameRules2048()
            .AddGameRulesThrees()
            .AddStats()
            .AddInfrastructure()
            .AddMiddlewares();

        services.AddHostedService<ConsoleHostedService>();
    }).UseSerilog()
    .RunConsoleAsync();