using Microsoft.Extensions.DependencyInjection;
using System;
using Serilog;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class AddSerilogServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
