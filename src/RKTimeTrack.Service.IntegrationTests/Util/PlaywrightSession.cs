using Microsoft.Playwright;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class PlaywrightSession : IAsyncDisposable
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    
    public IPage Page { get; }
    
    public PlaywrightSession(IPlaywright playwright, IBrowser browser, IPage page)
    {
        _playwright = playwright;
        _browser = browser;

        this.Page = page;
    }

    public async ValueTask DisposeAsync()
    {
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }
    
    // Helper methods from
    // https://github.com/microsoft/playwright-dotnet/blob/main/src/Playwright.NUnit/PlaywrightTest.cs
    public ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    public IPageAssertions Expect(IPage page) => Assertions.Expect(page);
    public IAPIResponseAssertions Expect(IAPIResponse response) => Assertions.Expect(response);
}