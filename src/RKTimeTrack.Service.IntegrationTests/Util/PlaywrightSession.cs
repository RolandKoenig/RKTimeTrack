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
}