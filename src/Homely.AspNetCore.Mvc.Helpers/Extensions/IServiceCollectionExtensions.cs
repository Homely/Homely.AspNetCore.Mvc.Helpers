using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
                                                          string title = "My API",
                                                          string version = "v1",
                                                          Func<ApiDescription, string> operationIdSelector = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(nameof(title));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException(nameof(version));
            }

            services.AddSwaggerGen(setupAction =>
            {
                // Do we wish to override the current operationId with some custom logic?
                if (operationIdSelector != null)
                {
                    setupAction.CustomOperationIds(operationIdSelector);
                }

                var info = new OpenApiInfo
                {
                    Title = title,
                    Version = version
                };

                setupAction.SwaggerDoc(version, info);
                    
            });

            return services;
        }
    }
}
