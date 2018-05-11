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
        private string _configuredCorsPolicy;

        public Startup(IConfiguration configuration)
        {
        }

        /// <remarks>This method can be overwritten in a test project to define configure CORS. If any CORS profiles are configured then return true, so CORS will then be wired up to use em.</remarks>
        public virtual string ConfigureCors(IServiceCollection services)
        {
            return null;
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
                options.WithGlobalAuthorization() // Everything is locked down by default.
                       .WithGlobalCancelledRequestHandler(); // Handle when a request is cancelled.
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
            app.UseAuthentication()
               .UseStaticFiles()
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
