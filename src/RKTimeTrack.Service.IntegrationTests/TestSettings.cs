namespace RKTimeTrack.Service.IntegrationTests;

internal static class TestSettings
{
    public const bool HEADLESS_MODE = true;
    public static readonly float? SLOW_MODE_MILLISECONDS = null;
    public static readonly TimeSpan DEFAULT_EXPECT_TIMEOUT = TimeSpan.FromSeconds(5);
}