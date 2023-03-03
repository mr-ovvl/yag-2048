using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yag2048.Core.Game;

namespace Yag2048;

internal sealed class ConsoleHostedService : IHostedService
{
    private readonly IGame _game;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger _logger;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime,
        IGame game)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _game = game ?? throw new ArgumentNullException(nameof(game));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting host with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    _logger.LogDebug("Starting app...");

                    await _game.Run();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                    throw;
                }
                finally
                {
                    _logger.LogDebug("Stopping application...");
                    _appLifetime.StopApplication();
                    _logger.LogDebug("...stopped successfully.");
                }
            }, cancellationToken);
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}