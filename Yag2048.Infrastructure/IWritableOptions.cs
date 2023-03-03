using Microsoft.Extensions.Options;

namespace Yag2048.Core.Infrastructure;

public interface IWritableOptions<TOptions> : IOptionsSnapshot<TOptions>, IOptionsMonitor<TOptions>
    where TOptions : class, new()
{
    void Update(TOptions changedValue, bool reload = false);
}