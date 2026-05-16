using NSubstitute;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class SecurityHeaderTests
{
    private readonly WebHostServerFixture _server;
    private readonly HttpClient _httpClient;
    
    public SecurityHeaderTests(
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
    public async Task Header_SERVER_is_not_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.False(response.Headers.Contains("Server"));
    }
    
    [Fact]
    public async Task CSP_Nonce_is_set_in_Index_file()
    {
        // Arrange
        var fakeNonce = "CSP-NONCE-1234567890";
        _server.CspNonceGeneratorMock.GetNonceForCurrentScope()
            .Returns(_ => fakeNonce);
        
        // Act
        var responseHtml = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        var fullContentHtml = await responseHtml.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.Contains(fakeNonce, fullContentHtml);
    }
    
    [Fact]
    public async Task CSP_Nonce_is_set_CSP_Header()
    {
        // Arrange
        var fakeNonce = "CSP-NONCE-1234567890";
        _server.CspNonceGeneratorMock.GetNonceForCurrentScope()
            .Returns(_ => fakeNonce);
        
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains(
            $"style-src 'self' 'nonce-{fakeNonce}'",
            response.Headers.GetValues("Content-Security-Policy").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_FRAME_OPTIONS_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_XSS_PROTECTION_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("0", response.Headers.GetValues("X-XSS-Protection").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_X_CONTENT_TYPE_OPTIONS_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_REFERRER_POLICY_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("strict-origin-when-cross-origin", response.Headers.GetValues("Referrer-Policy").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_CROSS_ORIGIN_EMBEDDER_POLICY_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("require-corp", response.Headers.GetValues("Cross-Origin-Embedder-Policy").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_CROSS_ORIGIN_RESOURCE_POLICY_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("same-site", response.Headers.GetValues("Cross-Origin-Resource-Policy").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_PERMISSIONS_POLICY_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var permissionPolicy = response.Headers.GetValues("Permissions-Policy").FirstOrDefault();
        Assert.Contains("geolocation=()", permissionPolicy);
        Assert.Contains("camera=()", permissionPolicy);
        Assert.Contains("microphone=()", permissionPolicy);
        Assert.Contains("interest-cohort=()", permissionPolicy);
    }
    
    [Fact]
    public async Task Header_X_ROBOTS_TAG_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("noindex, nofollow", response.Headers.GetValues("X-Robots-Tag").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_CONTENT_SECURITY_POLICY_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/index.html", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("default-src 'self'", response.Headers.GetValues("Content-Security-Policy").FirstOrDefault());
    }
    
    [Fact]
    public async Task Header_CACHE_CONTROL_is_set()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "/api/ui/state", 
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("no-store", response.Headers.GetValues("Cache-Control").FirstOrDefault());
    }
}