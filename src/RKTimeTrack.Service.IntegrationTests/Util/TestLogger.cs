using Microsoft.Extensions.Logging;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class TestLogger(WebHostServerFixture fixture) : ILogger
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
        var testOutputHelper = fixture.TestOutputHelper;
        testOutputHelper?.WriteLine(formatter(state, exception));
    }
}