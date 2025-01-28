using Microsoft.Playwright;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class PlaywrightSession : IAsyncDisposable
{
    public IPage Page { get; }
    
    public PlaywrightSession(IPage page)
    {
        this.Page = page;
    }

    public ValueTask DisposeAsync()
    {
        this.Page.CloseAsync();
        
        return ValueTask.CompletedTask;
    }
    
    // Helper methods from
    // https://github.com/microsoft/playwright-dotnet/blob/main/src/Playwright.NUnit/PlaywrightTest.cs
    public ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    public IPageAssertions Expect(IPage page) => Assertions.Expect(page);
    public IAPIResponseAssertions Expect(IAPIResponse response) => Assertions.Expect(response);
}