using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
            // Some simple JWT authentication.
            // NOTE: signing key is hardcoded because this is just a demo app.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pew pew pew pew pew pew"))
                        };
                    });

            services.AddMvcCore(options =>
                               {
                                   options.WithGlobalCancelledRequestHandler(); // Handle when a request is cancelled.
                                   options.WithEarlyWaningModelValidation(); // Error early if there's a bad model in the request.
                               })
                    .AddAHomeController(services, typeof(Startup), "pew pew")
                    .AddAuthorization()
                    .AddACommonJsonFormatter();

            ConfigureRepositories(services);
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

        // This method can be overwritten in a test project to define our repository.
        //          You would not do this in production, but we need to test our Helper library!
        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            // Create our fake data.
            var stubbedFakeVehicleRepository = StubbedFakeVehicleRepository.CreateAFakeVehicleRepository();

            // Inject our repo.
            services.AddSingleton<IFakeVehicleRepository>(stubbedFakeVehicleRepository);
        }

        // This method can be overwritten in a test project to define how the exception page is rendered.
        //          You would not do this in production, but we need to test our Helper library!
        public virtual void ConfigureJsonExceptionPages(IApplicationBuilder app)
        {
            app.UseJsonExceptionPages();
        }
    }
}
