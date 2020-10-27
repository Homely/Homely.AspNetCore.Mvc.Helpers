using System;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.ExtensionsTests.IServiceCollectionExtensionsTests
{
    public class AddCustomOpenApiTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData("someTitle", null)]
        [InlineData("someTitle", "")]
        [InlineData("someTitle", " ")]
        public void GivenSomeInvalidArguments_AddCustomOpenApi_ThrowsAnException(string title, string version)
        {
            // Arrange.
            var services = new ServiceCollection();

            // Act.
            var exception = Should.Throw<Exception>(() => services.AddCustomOpenApi(title, version, null));

            // Assert.
            exception.ShouldNotBeNull();
        }
    }
}
