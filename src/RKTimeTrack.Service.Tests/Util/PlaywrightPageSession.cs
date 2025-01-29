using Microsoft.Playwright;

namespace RKTimeTrack.Service.Tests.Util;

public class PlaywrightPageSession : IAsyncDisposable
{
    private IBrowserContext Context { get; }
    
    public IPage Page { get; }
    
    public PlaywrightPageSession(IBrowserContext context, IPage page)
    {
        this.Context = context;
        this.Page = page;
    }

    public async ValueTask DisposeAsync()
    {
        await this.Page.CloseAsync();
        await Context.DisposeAsync();
    }
    
    // Helper methods from
    // https://github.com/microsoft/playwright-dotnet/blob/main/src/Playwright.NUnit/PlaywrightTest.cs
    public ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    public IPageAssertions Expect(IPage page) => Assertions.Expect(page);
    public IAPIResponseAssertions Expect(IAPIResponse response) => Assertions.Expect(response);
}