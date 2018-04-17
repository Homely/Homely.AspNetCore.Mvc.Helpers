using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Homely.AspNetCore.Mvc.Helpers.Models;
using System.Collections.Generic;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.ExtensionsTests
{
    public class ApiErrorExtensionsTests
    {
        [Fact]
        public void GiveSomeErrorMessages_ToApiErrors_ReturnsSomeAPiErrors()
        {
            // Arrange.
            var errorMessages = new Dictionary<string, string>
            {
                { "Key1", "Message1" },
                { "Key2", "Message2" }
            };

            var expectedApiErrors = new List<ApiError>
            {
                new ApiError
                {
                    Key = "Key1",
                    Message = "Message1"
                },
                new ApiError
                {
                    Key = "Key2",
                    Message = "Message2"
                }
            };

            // Act.
            var apiErrors = errorMessages.ToApiErrors();

            // Assert.
            apiErrors.ShouldLookLike(expectedApiErrors);
        }
    }
}
