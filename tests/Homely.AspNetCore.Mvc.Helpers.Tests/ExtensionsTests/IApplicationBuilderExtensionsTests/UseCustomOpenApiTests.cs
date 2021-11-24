using System;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Builder;
using Moq;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.ExtensionsTests.IApplicationBuilderExtensionsTests
{
    public class UseCustomOpenApiTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", null, null)]
        [InlineData(" ", null, null)]
        [InlineData("someTitle", null, null)]
        [InlineData("someTitle", "", null)]
        [InlineData("someTitle", " ", null)]
        [InlineData("someTitle", "someVersion", null)]
        [InlineData("someTitle", "someVersion", "")]
        [InlineData("someTitle", "someVersion", " ")]
        public void GivenSomeInvalidArguments_UseCustomOpenApi_ThrowsAnException(string title,
                                                                                 string version,
                                                                                 string routePrefix)
        {
            // Arrange.
            var applicationBuilder = new Mock<IApplicationBuilder>().Object;

            OpenApiSettings openApiSettings = new()
            {
                Title = title,
                Version = version,
                RoutePrefix = routePrefix
            };

            // Act.
            var exception = Should.Throw<Exception>(() => applicationBuilder.UseCustomOpenApi(openApiSettings));

            // Assert.
            exception.ShouldNotBeNull();
        }
    }
}
