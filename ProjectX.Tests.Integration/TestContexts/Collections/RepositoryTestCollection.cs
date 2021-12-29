using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.TestContexts.Collections
{
    [CollectionDefinition(nameof(RepositoryTestCollection))]
    public class RepositoryTestCollection : ICollectionFixture<RepositoryTestFixture>
    {
    }
}