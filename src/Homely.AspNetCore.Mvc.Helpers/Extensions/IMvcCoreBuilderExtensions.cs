using Homely.AspNetCore.Mvc.Helpers.Controllers;
using Homely.AspNetCore.Mvc.Helpers.Helpers;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IMvcCoreBuilderExtensions
    {
        /// <summary>
        /// Specifies how any JSON output will default-render in the Response.
        /// </summary>
        /// <param name="mvcCoreBuilder">Configuring essential MVC services.</param>
        public static IMvcCoreBuilder AddACommonJsonFormatter(this IMvcCoreBuilder mvcCoreBuilder)
        {
            return mvcCoreBuilder.AddJsonFormatters(options =>
            {
                // Currently, there's no easy way to say: MVC pipelines use -this- instance of some JSS.
                // So, we have to manually set those values. Yes, if we add a new property setting (in the 
                // static class, then we have to add it here and yes ... we might forget to do that :(
                var settings = JsonHelpers.CreateJsonSerializerSettings();
                options.ContractResolver = settings.ContractResolver;
                options.Formatting = settings.Formatting;
                options.NullValueHandling = settings.NullValueHandling;
                options.DateFormatHandling = settings.DateFormatHandling;

                options.Converters = settings.Converters;
            });
        }

        /// <summary>
        /// Registers a common webapi home controller.
        /// </summary>
        /// <param name="builder">An interface for configuring essential MVC services.</param>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="callingType">Type of the calling assembly. This would usually be the main assembly of the web site.</param>
        /// <param name="asciiBanner">Optional: some text to display in the home controller output.</param>
        /// <returns>Chaining: the interface for configuring essential MVC services.</returns>
        public static IMvcCoreBuilder AddAHomeController(this IMvcCoreBuilder builder,
                                                         IServiceCollection services,
                                                         Type callingType,
                                                         string asciiBanner = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (callingType == null)
            {
                throw new ArgumentNullException(nameof(callingType));
            }

            var banner = new HomeControllerBanner(callingType, asciiBanner);
            services.AddSingleton<IHomeControllerBanner>(banner);

            builder.AddApplicationPart(typeof(HomeController).Assembly);

            return builder;
        }
    }
}