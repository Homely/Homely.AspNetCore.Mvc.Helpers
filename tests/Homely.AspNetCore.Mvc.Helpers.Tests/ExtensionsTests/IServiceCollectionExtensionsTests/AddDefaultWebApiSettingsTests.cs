using System;
using System.Collections.Generic;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.ExtensionsTests.IServiceCollectionExtensionsTests
{
    public class AddDefaultWebApiSettingsTests
    {
        [Fact]
        public void GivenSomeSmallAmountOfData_AddDefaultWebApiSettings_ShouldSetupTheSettings()
        {
            // Arrange.
            var services = new ServiceCollection();

            // Act.
            var sameServices = services.AddDefaultWebApiSettings(
                "some banner",
                true,
                "some title",
                "some apiVersion");

            // Assert.
            sameServices.ShouldNotBeNull();
        }

        public static TheoryData<string?, IEnumerable<Action<IMvcBuilder>>?> Data => new()
            {
                // Nothing.
                {
                    null,
                    null
                },

                // Banner but no method chaining.
                {
                    "some banner",
                    null
                },

                // Empty method chaining.
                {
                    "some banner",
                    new List<Action<IMvcBuilder>>()
                },

                // Banner + 1 other method.
                {
                     "some banner",
                     new List<Action<IMvcBuilder>>()
                     {
                         new Action<IMvcBuilder>(_ => { var i = 1; i++; })
                     }
                },

                // Banner + multiple other methods.
                {
                     "some banner",
                     new List<Action<IMvcBuilder>>()
                     {
                         new Action<IMvcBuilder>(_ => { var i = 1; i++; }),
                         new Action<IMvcBuilder>(_ => { var j = 1; j++; }),
                         new Action<IMvcBuilder>(_ => { var k = 1; k++; })
                     }
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void GivenSomeSettings_AddDefaultWebApiSettings_ShouldSetupTheSettings(string? banner,
            IEnumerable<Action<IMvcBuilder>>? otherMethods)
        {
            // Arrange.
            var services = new ServiceCollection();

            // Act.
            var sameServices = services.AddDefaultWebApiSettings(
                banner: banner,
                false,
                false,
                null,
                null,
                otherChainedMethods: otherMethods);

            // Assert.
            sameServices.ShouldNotBeNull();
        }

        [Fact]
        public void GivenLargishAmountOfData_AddDefaultWebApiSettings_ShouldSetupTheSettings()
        {
            // Arrange.
            var services = new ServiceCollection();

            // Act.
            var sameServices = services.AddDefaultWebApiSettings(
                "some banner",
                true,
                true,
                null,
                "some title",
                "some apiVersion",
                null);

            // Assert.
            sameServices.ShouldNotBeNull();
        }

        [Fact]
        public void GivenTheFullData_AddDefaultWebApiSettings_ShouldSetupTheSettings()
        {
            // Arrange.
            var services = new ServiceCollection();

            // Act.
            var sameServices = services.AddDefaultWebApiSettings(
                "some banner",
                true,
                true,
                null,
                null,
                null);

            // Assert.
            sameServices.ShouldNotBeNull();
        }
    }
}
