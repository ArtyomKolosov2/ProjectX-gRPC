using System;
using System.Threading.Tasks;
using ProjectX.DataAccess.Context;
using ProjectX.DataAccess.Context.Base;
using ProjectX.Tests.Integration.Fixtures;
using Xunit;

namespace ProjectX.Tests.Integration.TestContext
{
    public class RepositoryTestContext
    {
        public RepositoryTestContext()
        {
            DbFixture = new DbFixture();
        }
        public DbFixture DbFixture { get; set; }
    }
}