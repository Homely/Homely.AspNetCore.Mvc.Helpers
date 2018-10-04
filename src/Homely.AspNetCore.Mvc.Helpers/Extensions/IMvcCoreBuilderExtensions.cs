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
            return mvcCoreBuilder.AddJsonFormatters(settings =>
            {
                settings = JsonHelpers.JsonSerializerSettings;
            });
        }

        /// <summary>
        /// Registers a common webapi home controller.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="services"></param>
        /// <param name="callingType"></param>
        /// <param name="asciiBanner"></param>
        /// <returns></returns>
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