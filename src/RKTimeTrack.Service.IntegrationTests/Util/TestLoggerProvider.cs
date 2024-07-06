using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class TestLoggerProvider(ITestOutputHelper testOutputHelper) : ILoggerProvider
{
    /// <inheritdoc />
    public void Dispose()
    {
        
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(testOutputHelper);
    }
}