﻿using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestWebApplication.Repositories;

namespace TestWebApplication
{
    public class Startup
    {
        private IConfiguration configuration;
        private string _configuredCorsPolicy;
        
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <remarks>This method can be overwritten in a test project to define configure CORS. If any CORS profiles are configured then return true, so CORS will then be wired up to use em.</remarks>
        public virtual string ConfigureCors(IServiceCollection services)
        {
            return null;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                options.WithGlobalAuthorization()
                       .WithGlobalCancelledRequestHandler();
            })
                    .AddAuthorization()
                    .AddACommonJsonFormatter()
                    .AddCustomFluentValidation(typeof(Startup));

            _configuredCorsPolicy = ConfigureCors(services);
            ConfigureRepositories(services);
        }

        /// <remarks>This method can be overwritten in a test project to define a stubbed/pre-seeded repository.</remarks>
        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            // Create our fake data.
            var stubbedFakeVehicleRepository = StubbedFakeVehicleRepository.CreateAFakeVehicleRepository();

            // Inject our repo.
            services.AddSingleton<IFakeVehicleRepository>(stubbedFakeVehicleRepository);
        }

        /// <remarks>This method can be overwritten in a test project to define how the exception page is rendered.
        ///          You would not do this in production, but we need to test our Helper library!</remarks>
        public virtual void ConfigureJsonExceptionPages(IApplicationBuilder app)
        {
            app.UseJsonExceptionPages(_configuredCorsPolicy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles()
               .UseStatusCodePages();

            // Allows the unit tests to define custom unit test logic for the json exception pages.
            ConfigureJsonExceptionPages(app);
            
            if (!string.IsNullOrWhiteSpace(_configuredCorsPolicy))
            {
                app.UseCors(_configuredCorsPolicy);
            }

            app.UseMvc();
        }
    }
}
