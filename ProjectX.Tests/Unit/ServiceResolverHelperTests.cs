using System;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ProjectX.BusinessLayer.GrpcServices;
using ProjectX.BusinessLayer.Services.Files;
using ProjectX.Tests.Helpers;
using Xunit;

namespace ProjectX.Tests.Unit
{
    [Collection("UnitTests")]
    public class ServiceResolverHelperTests
    {
        private readonly IWebHost _webHost;
        private interface ITestInterface { }

        public ServiceResolverHelperTests()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
        }
        [Fact]
        public void ServiceExists_TryToGetService_Success()
        {
            // Arrange
            var serverResolver = new ServiceResolverHelper(_webHost);
            
            // Act
            var requestedService = serverResolver.GetService<GridFsFileService>();
            
            // Assert
            requestedService.Should().NotBeNull();
        }
        
        [Fact]
        public void ServiceNotExists_TryToGetService_Failed()
        {
            // Arrange
            var serverResolver = new ServiceResolverHelper(_webHost);
            
            // Act
            Action requestedService = () => serverResolver.GetService<ITestInterface>();
            
            // Assert
            requestedService.Should().Throw<InvalidOperationException>();
        }
    }
}