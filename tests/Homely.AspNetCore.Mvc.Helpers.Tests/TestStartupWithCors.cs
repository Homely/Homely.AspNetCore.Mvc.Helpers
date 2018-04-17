using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestWebApplication;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class TestStartupWithCors : Startup
    {
        public TestStartupWithCors(IConfiguration configuration) : base(configuration)
        {
        }

        public override string ConfigureCors(IServiceCollection services)
        {
            var corsPolicy = Guid.NewGuid().ToString();

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, builder => builder.AllowAnyOrigin()
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod());
            });

            return corsPolicy;
        }
    }
}
