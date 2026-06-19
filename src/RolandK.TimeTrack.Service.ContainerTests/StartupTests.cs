using RolandK.TimeTrack.Service.ContainerTests.Util;

namespace RolandK.TimeTrack.Service.ContainerTests;

[Collection(nameof(TestEnvironmentCollection))]
public class StartupTests(TestEnvironmentFixture fixture, ITestOutputHelper output)
{
    private static readonly HttpClient HttpClient = new();

    [Fact]
    [Trait("Category", "NeedsDocker")]
    public async Task Start_application_container_and_serve_index_page()
    {
        try
        {
            await fixture.EnsureContainersLoadedAsync(output);

            // Act
            var response = await HttpClient.GetAsync(
                fixture.AppBaseUrl, TestContext.Current.CancellationToken);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("<html", responseContent);
        }
        finally
        {
            await fixture.WriteAppLogsAsync(output.WriteLine);
        }
    }
    
    [Fact]
    [Trait("Category", "NeedsDocker")]
    public async Task Start_application_container_and_check_test_endpoint()
    {
        try
        {
            await fixture.EnsureContainersLoadedAsync(output);

            // Act
            var response = await HttpClient.GetAsync(
                fixture.AppBaseUrl + "/secrets-test", TestContext.Current.CancellationToken);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
        finally
        {
            await fixture.WriteAppLogsAsync(output.WriteLine);
        }
    }
}