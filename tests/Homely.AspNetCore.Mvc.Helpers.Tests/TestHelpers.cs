using Homely.AspNetCore.Mvc.Helpers.Helpers;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Newtonsoft.Json;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public static class TestHelpers
    {
        /// <summary>
        /// Compares two models and throws an exception if they are not 'equal'.
        /// </summary>
        /// <typeparam name="T">Can be any POCO.</typeparam>
        /// <param name="actual">Model to test again. i.e. Source model.</param>
        /// <param name="expected">Model which contains the expected structure/data. i.e. destination model.</param>
        /// <remarks>This extension method is mainly to be used during an <code>Assert</code> test section.</remarks>
        public static void ShouldLookLike<T>(this T actual, T expected)
        {
            if (actual == null &&
                expected == null)
            {
                return;
            }

            var actualJson = JsonConvert.SerializeObject(actual, JsonHelpers.JsonSerializerSettings);
            ShouldLookLike(actualJson, expected);
        }

        /// <summary>
        /// Compares a string (which should be json) and a model and throws an exception if they are not 'equal'.
        /// </summary>
        /// <typeparam name="T">Can be any POCO.</typeparam>
        /// <param name="content">Source HttpContent to extract string-content from.</param>
        /// <param name="expected">Model which contains the expected structure/data. i.e. destination model.</param>
        /// <remarks>This extension method is mainly to be used during an <code>Assert</code> test section.</remarks>
        public static async Task ShouldLookLike<T>(this HttpContent content, T expected)
        {
            content.ShouldNotBeNull();

            var responseJson = await content.ReadAsStringAsync();
            ShouldLookLike(responseJson, expected);
        }

        public static ErrorViewModel CreateAnApiError(string key, string message)
        {
            message.ShouldNotBeNullOrWhiteSpace();

            var apiErrors = new[]
            {
                new ApiError
                {
                    Key = key, // Optional.
                    Message = message
                }
            };

            return new ErrorViewModel(apiErrors);
        }

        private static void ShouldLookLike<T>(string actual, T expected)
        {
            if (string.IsNullOrWhiteSpace(actual) &&
                expected == null)
            {
                return;
            }

            var expectedJson = JsonConvert.SerializeObject(expected, JsonHelpers.JsonSerializerSettings);
            actual.ShouldBe(expectedJson);
        }
    }
}
