using Microsoft.Extensions.Logging;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class TestLoggerProvider(WebHostServerFixture fixture) : ILoggerProvider
{
    /// <inheritdoc />
    public void Dispose()
    {
        
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(fixture);
    }
}