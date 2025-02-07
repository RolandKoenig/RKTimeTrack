using Serilog.Core;
using Serilog.Events;

namespace RolandK.TimeTrack.Service.Tests.Util;

public class TestLoggerSink(WebHostServerFixture fixture) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var testOutputHelper = fixture.TestOutputHelper;
        testOutputHelper?.WriteLine($"{DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm:ss.fff} {logEvent.Level} {logEvent.RenderMessage()}");
    }
}