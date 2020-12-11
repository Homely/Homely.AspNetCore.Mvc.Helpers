using System;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.ExtensionsTests.IMvcBuilderExtensionsTests
{
    public class AddAHomeControllerTests
    {
        [Fact]
        public void GivenNoServiceCollection_AddAHomeController_ThrowsAnException()
        {
            // Arrange.
            var mvcBuilder = new Mock<IMvcBuilder>().Object;            

            // Act.
            var exception = Should.Throw<Exception>(() => mvcBuilder.AddAHomeController(null));

            // Assert.
            exception.ShouldNotBeNull();
        }
    }
}
