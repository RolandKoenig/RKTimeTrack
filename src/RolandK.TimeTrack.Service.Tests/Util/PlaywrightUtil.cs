namespace RolandK.TimeTrack.Service.Tests.Util;

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