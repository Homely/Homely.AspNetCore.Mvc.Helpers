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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GivenSomeInvalidArguments_AddAHomeController_ThrowsAnException(string asciiBanner)
        {
            // Arrange.
            var mvcBuilder = new Mock<IMvcBuilder>().Object;
            var serviceCollection = new Mock<IServiceCollection>();

            // Act.
            var exception = Should.Throw<Exception>(() => mvcBuilder.AddAHomeController(serviceCollection.Object,
                                                                                        asciiBanner));

            // Assert.
            exception.ShouldNotBeNull();
        }
    }
}
