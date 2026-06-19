namespace RolandK.TimeTrack.Service.Api.Tests;

public static class TestEndpointExtensions
{
    public static T MapTestEndpoint<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        endpointBuilder.MapGet("/secrets-test", () => Results.Ok());
        return endpointBuilder;
    }
}