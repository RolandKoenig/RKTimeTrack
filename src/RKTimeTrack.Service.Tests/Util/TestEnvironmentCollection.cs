using Xunit;

namespace RKTimeTrack.Service.Tests.Util;

[CollectionDefinition(nameof(TestEnvironmentCollection))]
public class TestEnvironmentCollection : ICollectionFixture<WebHostServerFixture> { }