using Hellang.Middleware.ProblemDetails;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestWebApplication.Repositories;

namespace TestWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                               {
                                   options.WithGlobalCancelledRequestHandler(); // Handle a user-cancelled request.
                               })
                    .AddAHomeController(services, typeof(Startup), "pew pew")
                    .AddAuthorization()
                    .AddACommonJsonFormatter()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Invalid model states display ProblemDetails as their model.
            services.ConfigureInvalidModelStateProblemDetails();

            ConfigureRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseProblemDetails(x => x.IncludeExceptionDetails = _ => env.IsDevelopment());

            app.UseMvc();
        }

        // This method can be overwritten in a test project to define our repository.
        //          You would not do this in production, but we need to test our Helper library!
        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            // Create our fake data.
            var stubbedFakeVehicleRepository = StubbedFakeVehicleRepository.CreateAFakeVehicleRepository();

            // Inject our repo.
            services.AddSingleton<IFakeVehicleRepository>(stubbedFakeVehicleRepository);
        }
    }
}
