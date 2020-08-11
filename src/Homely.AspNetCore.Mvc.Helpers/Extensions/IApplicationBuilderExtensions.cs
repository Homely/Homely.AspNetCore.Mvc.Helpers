using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        private const string DefaultSwaggerTitle = "My API";
        private const string DefaultSwaggerVersion = "v1";
        private const string DefaultSwaggerRoutePrefex = "swagger";

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder application,
                                                           string title = DefaultSwaggerTitle,
                                                           string version = DefaultSwaggerVersion,
                                                           string routePrefix = DefaultSwaggerRoutePrefex)
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
        /// - UseCustomSwagger<br/>
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
                                                                   string title = DefaultSwaggerTitle,
                                                                   string version = DefaultSwaggerVersion,
                                                                   string routePrefix = DefaultSwaggerRoutePrefex)
        {
            application.UseProblemDetails()
                       .UseCustomSwagger(title, version, routePrefix)
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
