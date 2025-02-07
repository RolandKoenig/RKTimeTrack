using Xunit;

namespace RolandK.TimeTrack.Service.Tests.Util;

[CollectionDefinition(nameof(TestEnvironmentCollection))]
public class TestEnvironmentCollection : ICollectionFixture<WebHostServerFixture> { }