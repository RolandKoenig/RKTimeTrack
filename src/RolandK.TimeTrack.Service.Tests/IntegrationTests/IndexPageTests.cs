using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class IndexPageTests
{
    private readonly WebHostServerFixture _server;
    private readonly HttpClient _httpClient;
    
    public IndexPageTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = _server.RootUri;
    }
    
    [Theory]
    [InlineData("/index.html")]
    [InlineData("/index.htm")]
    [InlineData("/")]
    [InlineData("/index")]
    [InlineData("/some/other/page")]
    public async Task GetIndexPage(string url)
    {
        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        
        // Assert
        Assert.Contains("<html", responseText);
        Assert.Contains("</html>", responseText);
        Assert.Contains("""<div id="app"></div>""", responseText);
    }
}