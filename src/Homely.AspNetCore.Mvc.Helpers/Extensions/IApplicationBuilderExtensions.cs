using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        private const string DefaultOpenApiTitle = "My API";
        private const string DefaultOpenApiVersion = "v1";
        private const string DefaultOpenApiRoutePrefex = "swagger";

        public static IApplicationBuilder UseCustomOpenApi(this IApplicationBuilder application,
                                                           string title = DefaultOpenApiTitle,
                                                           string version = DefaultOpenApiVersion,
                                                           string routePrefix = DefaultOpenApiRoutePrefex)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(nameof(title));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException(nameof(version));
            }

            if (string.IsNullOrWhiteSpace(routePrefix))
            {
                throw new ArgumentException(nameof(routePrefix));
            }

            application.UseSwagger();
            
            application.UseSwaggerUI(c =>
            {
                // e.g. /swagger/v1/swagger.json
                c.SwaggerEndpoint($"/{routePrefix}/{version}/swagger.json", title);
            });

            return application;
        }

        /// <summary>
        /// This menthod uses the common Web Api settings:<br/>
        /// - UseProblemDetails<br/>
        /// - UseCustomOpenApi<br/>
        /// - UseRouting<br/>
        /// - OPTIONAL: UseAuthorization<br/>
        /// - UseEndpoints that map controllers<br/>
        /// </summary>
        /// <param name="application"></param>
        /// <param name="useAuthorization"></param>
        /// <param name="title"></param>
        /// <param name="version"></param>
        /// <param name="routePrefix"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDefaultWebApiSettings(this IApplicationBuilder application,
                                                                   bool useAuthorization = true,
                                                                   string title = DefaultOpenApiTitle,
                                                                   string version = DefaultOpenApiVersion,
                                                                   string routePrefix = DefaultOpenApiRoutePrefex)
        {
            application.UseProblemDetails()
                       .UseCustomOpenApi(title, version, routePrefix)
                       .UseRouting();

            if (useAuthorization)
            {
                application.UseAuthorization();
            }

            application.UseEndpoints(endpoints => endpoints.MapControllers());

            return application;
        }
    }
}
