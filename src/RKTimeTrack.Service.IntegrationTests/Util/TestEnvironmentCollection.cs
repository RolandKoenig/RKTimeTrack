namespace RKTimeTrack.Service.IntegrationTests.Util;

[CollectionDefinition(nameof(TestEnvironmentCollection))]
public class TestEnvironmentCollection : ICollectionFixture<WebHostServerFixture> { }