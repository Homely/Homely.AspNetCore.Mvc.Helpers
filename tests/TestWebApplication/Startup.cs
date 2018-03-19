using Homely.AspNetCore.Mvc.Helpers.Extensions;
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

        /// <remarks>This method can be overwitten in a test project to define how to custom handle any exceptions in a test.</remarks>
        public virtual void ConfigureUseAllResponsesAsJson(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseAllResponsesAsJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // NOTE: order is important here..
            ConfigureUseAllResponsesAsJson(app);
            app.UseStaticFiles()
               .UseMvc();
        }
    }
}
