namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    public record OpenApiSettings
    {
        public  const string DefaultOpenApiTitle = "My API";
        public const string DefaultOpenApiVersion = "v1";
        public const string DefaultOpenApiRoutePrefex = "swagger";

        public string Title { get; init; } = DefaultOpenApiTitle;
        public string Version { get; init; } = DefaultOpenApiVersion;
        public string RoutePrefix { get; init; } = DefaultOpenApiRoutePrefex;
    }
}

// Below is need for records to work on netcoreapp3.1
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}
