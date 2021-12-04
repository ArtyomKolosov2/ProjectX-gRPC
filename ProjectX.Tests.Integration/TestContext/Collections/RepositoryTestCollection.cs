using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.TestContext.Collections
{
    [CollectionDefinition(nameof(RepositoryTestCollection))]
    public class RepositoryTestCollection : ICollectionFixture<RepositoryTestContext>
    {
        
    }
}