using System.Net;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class HealthApiTests
{
    private readonly WebHostServerFixture _server;
    private readonly HttpClient _httpClient;

    public HealthApiTests(
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

    [Fact]
    public async Task Health_endpoint_returns_ok()
    {
        // Act
        var response = await _httpClient.GetAsync("health", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}