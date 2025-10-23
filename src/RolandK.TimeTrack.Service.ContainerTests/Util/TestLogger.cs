using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.ContainerTests.Util;

public class TestLogger(ITestOutputHelper testOutputHelper) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        testOutputHelper.WriteLine(
            $"{logLevel} {formatter(state, exception)}");
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel > LogLevel.Debug;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}