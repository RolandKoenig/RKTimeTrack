using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class TestLogger(ITestOutputHelper testOutputHelper) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        testOutputHelper.WriteLine(formatter(state, exception));
    }
}