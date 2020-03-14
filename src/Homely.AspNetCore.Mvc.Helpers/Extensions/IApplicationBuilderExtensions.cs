using Microsoft.AspNetCore.Builder;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder application,
                                                           string routePrefix = "swagger",
                                                           string title = "My API",
                                                           string version = "v1")
        {
            if (string.IsNullOrWhiteSpace(routePrefix))
            {
                throw new ArgumentException(nameof(routePrefix));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException(nameof(version));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(nameof(title));
            }

            application.UseSwagger();
            //application.UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{routePrefix}/{version}/swagger.json", title);
                //c.RoutePrefix = routePrefix;
            });

            return application;
        }
    }
}
