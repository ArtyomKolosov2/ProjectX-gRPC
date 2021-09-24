using ProjectX.BusinessLayer.Services.Files;
using ProjectX.Tests.Helpers;

namespace ProjectX.Tests.TestContext
{
    public class TestContext
    {
        private readonly ServiceResolverHelper _serviceResolverHelper;

        public TestContext(ServiceResolverHelper serviceResolverHelper)
        {
            _serviceResolverHelper = serviceResolverHelper;
        }

        private GridFsFileService GridFsFileService => _serviceResolverHelper.GetService<GridFsFileService>();
    }
}