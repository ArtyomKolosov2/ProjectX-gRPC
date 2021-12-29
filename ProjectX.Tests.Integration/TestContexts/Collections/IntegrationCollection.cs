using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.TestContexts.Collections
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture> 
    {
    }
}