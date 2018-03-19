using FluentValidation.AspNetCore;
using Homely.AspNetCore.Mvc.Helpers.ActionFilters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

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
        /// Registers all FluentValidation validators found in all assemblies then wires up auto model validation against these validators.
        /// </summary>
        public static IMvcCoreBuilder AddCustomFluentValidation(this IMvcCoreBuilder mvcCoreBuilder, IEnumerable<Type> types)
        {
            if (mvcCoreBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcCoreBuilder));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            return mvcCoreBuilder.AddFluentValidation(options =>
                                 {
                                     foreach(var type in types)
                                     {
                                        options.RegisterValidatorsFromAssemblyContaining(type);
                                     }
                                     options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                                 })
                                 .AddMvcOptions(options => options.Filters.Add(new ValidateModelAttribute()));
        }

        /// <summary>
        /// Registers all FluentValidation validators found in a single assembly and then wires up auto model validation against these validators.
        /// </summary>
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
