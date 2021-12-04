using Xunit;

namespace ProjectX.Tests.Integration.TestContext.Collections
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestContext> 
    {
    }
}