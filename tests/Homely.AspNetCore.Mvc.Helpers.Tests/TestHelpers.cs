using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public static class TestHelpers
    {
        private static readonly Lazy<JsonSerializerOptions> _options = new(() =>
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return options;
        });

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

            var actualJson = JsonSerializer.Serialize(actual, _options.Value);
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

        public static async Task ShouldHaveSameProblemDetails(this HttpContent content, ProblemDetails expectedProblemDetails)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (expectedProblemDetails == null)
            {
                throw new ArgumentNullException(nameof(expectedProblemDetails));
            }

            var problemDetails = await DeserializeAProblemDetailAsync<ProblemDetails>(content);

            problemDetails.Type.ShouldBe(expectedProblemDetails.Type);
            problemDetails.Title.ShouldBe(expectedProblemDetails.Title);
            problemDetails.Status.ShouldBe(expectedProblemDetails.Status);
        }

        public static async Task ShouldHaveSameProblemDetails(this HttpContent content, ValidationProblemDetails expectedProblemDetails)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (expectedProblemDetails == null)
            {
                throw new ArgumentNullException(nameof(expectedProblemDetails));
            }

            await content.ShouldHaveSameProblemDetails(expectedProblemDetails as ProblemDetails);
            var problemDetails = await DeserializeAProblemDetailAsync<ValidationProblemDetails>(content);

            problemDetails.Errors.ShouldLookLike(expectedProblemDetails.Errors);
        }

        private static void ShouldLookLike<T>(string actual, T expected)
        {
            if (string.IsNullOrWhiteSpace(actual) &&
                expected == null)
            {
                return;
            }

            var expectedJson = JsonSerializer.Serialize(expected, _options.Value);
            actual.ShouldBe(expectedJson);
        }

        private static async Task<T> DeserializeAProblemDetailAsync<T>(HttpContent content) where T : ProblemDetails
        {
            var responseJson = await content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(responseJson);

            if (result is null)
            {
                throw new Exception($"Failed to convert content to a strongly-typed instance. Content: [{responseJson}].");
            }

            return result;
        }
    }
}
