using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        private const string DefaultOpenApiTitle = "My API";
        private const string DefaultOpenApiVersion = "v1";

        public static IServiceCollection AddDefaultWebApiSettings(this IServiceCollection services,
                                                                  string title = DefaultOpenApiTitle,
                                                                  string version = DefaultOpenApiVersion)
        {
            return services.AddDefaultWebApiSettings(default,
                default,
                default,
                default,
                title,
                version,
                default);
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
        /// <param name="isJsonIndented"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultWebApiSettings(this IServiceCollection services,
                                                                  string? banner = null,
                                                                  bool includeExceptionDetails = false,
                                                                  bool isJsonIndented = false)
        {
            return services.AddDefaultWebApiSettings(banner,
                includeExceptionDetails,
                isJsonIndented,
                default,
                DefaultOpenApiTitle,
                DefaultOpenApiVersion,
                default);
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
        /// <param name="isJsonIndented"></param>
        /// <param name="jsonDateTimeFormat"></param>
        /// <param name="title"></param>
        /// <param name="version"></param>
        /// <param name="otherChainedMethods"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultWebApiSettings(this IServiceCollection services,
                                                                  string? banner = null,
                                                                  bool includeExceptionDetails = false,
                                                                  bool isJsonIndented = false,
                                                                  string? jsonDateTimeFormat = null,
                                                                  string title = DefaultOpenApiTitle,
                                                                  string version = DefaultOpenApiVersion,
                                                                  IEnumerable<Action<IMvcBuilder>>? otherChainedMethods = null)
        {

            var swaggerGenerationOptions = CreateCustomOpenApi(title, version);

            return services.AddDefaultWebApiSettings(banner,
                includeExceptionDetails,
                isJsonIndented,
                jsonDateTimeFormat,
                swaggerGenerationOptions,
                otherChainedMethods);
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
        /// <param name="isJsonIndented"></param>
        /// <param name="jsonDateTimeFormat"></param>
        /// <param name="customSwaggerGenerationOptions"></param>
        /// <param name="otherChainedMethods"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultWebApiSettings(this IServiceCollection services,
                                                                  string? banner = null,
                                                                  bool includeExceptionDetails = false,
                                                                  bool isJsonIndented = false,
                                                                  string? jsonDateTimeFormat = null,
                                                                  Action<SwaggerGenOptions>? customSwaggerGenerationOptions = null,
                                                                  IEnumerable<Action<IMvcBuilder>>? otherChainedMethods = null)
        {
            var mvcBuilder = services.AddControllers();

            mvcBuilder
                .AddAHomeController(services, banner, Assembly.GetCallingAssembly())
                .AddDefaultJsonOptions(isJsonIndented, jsonDateTimeFormat);

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => includeExceptionDetails;
            });

            // Use the provided action OR use a default one.
            if (customSwaggerGenerationOptions != null)
            {
                services.AddSwaggerGen(customSwaggerGenerationOptions);
            }

            if (otherChainedMethods != null &&
                otherChainedMethods.Any())
            {
                foreach(var method in otherChainedMethods)
                {
                    method(mvcBuilder);
                }
            }

            return services;
        }

        private static Action<SwaggerGenOptions> CreateCustomOpenApi(string title = DefaultOpenApiTitle,
                                                                     string version = DefaultOpenApiVersion)
        {
            return new Action<SwaggerGenOptions>(setupAction =>
            {
                setupAction.CustomOperationIds(CustomOperationIdSelector);

                var info = new OpenApiInfo
                {
                    Title = title,
                    Version = version
                };

                setupAction.SwaggerDoc(version, info);
            });
        }

        // Format: <Controller>_<HTTP Method>_<MethodName>
        // E.g. : Home_GET_SearchAsync
        private static string CustomOperationIdSelector(ApiDescription apiDescription)
        {
            var controllerName = ((ControllerActionDescriptor)apiDescription.ActionDescriptor).ControllerName;
            var methodName = apiDescription.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : $"Unknown_Method_Name_{Guid.NewGuid()}";
            return $"{controllerName}_{apiDescription.HttpMethod}_{methodName}";
        }
    }
}
