using RolandK.TimeTrack.Service.ContainerTests.Util;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.ContainerTests;

[Collection(nameof(TestEnvironmentCollection))]
public class StartupTests(TestEnvironmentFixture fixture, ITestOutputHelper output)
{
    [Fact]
    [Trait("Category", "NeedsDocker")]
    public async Task Start_application_container_and_serve_index_page()
    {
        try
        {
            await fixture.EnsureContainersLoadedAsync();

            // Act
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(fixture.AppBaseUrl);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("<html", responseContent);
        }
        finally
        {
            await fixture.WriteAppLogsAsync(output.WriteLine);
        }
    }
}