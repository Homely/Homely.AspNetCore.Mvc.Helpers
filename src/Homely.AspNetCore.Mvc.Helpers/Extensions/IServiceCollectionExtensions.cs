﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
                                                          string title = "My API",
                                                          string version = "v1")
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new System.ArgumentException(nameof(title));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new System.ArgumentException(nameof(version));
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version,
                    new OpenApiInfo
                    {
                        Title = title,
                        Version = version
                    });
            });

            return services;
        }
    }
}
