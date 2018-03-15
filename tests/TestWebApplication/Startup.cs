using Homely.AspNetCore.Mvc.Helpers.ActionFilters;
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
            services.AddMvcCore(config => { config.Filters.Add(new ValidateModelAttribute()); })
                    .AddACommonJsonFormatter();

            ConfigureRepositories(services);
        }

        /// <remarks>This method can be overwritten in a test project to define a stubbed/pre-seeded repository.</remarks>
        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IFakeVehicleRepository, FakeVehicleRepository>();
        }

        public virtual void ConfigureJsonExceptionPage(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseJsonExceptionPage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            ConfigureJsonExceptionPage(app);

            app.UseMvc();
        }
    }
}
