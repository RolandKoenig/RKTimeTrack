using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class SecurityHeaderTests
{
    private readonly WebHostServerFixture _server;
    
    public SecurityHeaderTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }
    
    [Fact]
    public async Task Header_XSS_PROTECTION_is_set_to_0()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        var response = await httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("0", response.Headers.GetValues("X-XSS-Protection").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_FRAME_OPTIONS_is_set_to_deny()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        var response = await httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").FirstOrDefault());
    }
}