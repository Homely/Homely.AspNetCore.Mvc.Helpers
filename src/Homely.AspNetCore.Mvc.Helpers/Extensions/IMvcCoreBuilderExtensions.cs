using FluentValidation.AspNetCore;
using Homely.AspNetCore.Mvc.Helpers.ActionFilters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class IMvcCoreBuilderExtensions
    {
        /// <summary>
        /// Specifies how any JSON output will default-render in the Response.
        /// </summary>
        public static IMvcCoreBuilder AddACommonJsonFormatter(this IMvcCoreBuilder mvcCoreBuilder)
        {
            return mvcCoreBuilder.AddJsonFormatters(options =>
            {
                options.NullValueHandling = NullValueHandling.Ignore;
                options.Formatting = Formatting.Indented;
                options.Converters.Add(new StringEnumConverter());
            });
        }

        /// <summary>
        /// Registers 
        /// </summary>
        /// <param name="mvcCoreBuilder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddCustomFluentValidation(this IMvcCoreBuilder mvcCoreBuilder, Type type)
        {
            if (mvcCoreBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcCoreBuilder));
            }

            return mvcCoreBuilder.AddFluentValidation(options =>
                                 {
                                     options.RegisterValidatorsFromAssemblyContaining(type);
                                     options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                                 })
                                 .AddMvcOptions(options => options.Filters.Add(new ValidateModelAttribute()));
        }
    }
}
