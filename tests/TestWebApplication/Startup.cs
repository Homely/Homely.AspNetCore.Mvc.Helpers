using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestWebApplication.Repositories;

namespace TestWebApplication
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddACommonJsonFormatter()
                    .AddCustomFluentValidation(typeof(Startup));

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
            app.UseJsonExceptionPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles()
               .UseStatusCodePages();

            // Allows the unit tests to define custom unit test logic for the json exception pages.
            ConfigureJsonExceptionPages(app);
            
            app.UseMvc();
        }
    }
}
