using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        private const string DefaultSwaggerTitle = "My API";
        private const string DefaultSwaggerVersion = "v1";

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
                                                          string title = DefaultSwaggerTitle,
                                                          string version = DefaultSwaggerVersion,
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

        /// <summary>
        /// This method adds the common web api services:<br/>
        /// - AddControllers<br/>
        /// - AddHomeController<br/>
        /// - AddDefaultJsonOptions<br/>
        /// - AddProblemDeatils<br/>
        /// - AddCustomSwagger<br/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="banner"></param>
        /// <param name="includeExceptionDetails"></param>
        /// <param name="title"></param>
        /// <param name="version"></param>
        /// <param name="operationIdSelector"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultWebApiSettings(this IServiceCollection services,
                                                                  string banner,
                                                                  bool includeExceptionDetails,
                                                                  string title = DefaultSwaggerTitle,
                                                                  string version = DefaultSwaggerVersion,
                                                                  Func<ApiDescription, string> operationIdSelector = null)
        {
            services.AddControllers()
                    .AddAHomeController(services, banner)
                    .AddDefaultJsonOptions();

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => includeExceptionDetails;
            })
                    .AddCustomSwagger(title,
                                      version,
                                      operationIdSelector);

            return services;

        }
    }
}
