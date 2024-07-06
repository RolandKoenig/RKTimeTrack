namespace RKTimeTrack.Service.IntegrationTests.Util;

internal static class PlaywrightUtil
{
    internal static void EnsureBrowsersInstalled()
    {
        var exitCode = Microsoft.Playwright.Program.Main(["install"]);
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode}");
        }
    }
}