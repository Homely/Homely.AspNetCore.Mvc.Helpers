﻿using Hellang.Middleware.ProblemDetails;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using TestWebApplication.Repositories;

namespace TestWebApplication
{
    public class Startup
    {
        private const string SwaggerVersion = "v2";

        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                               {
                                   options.WithGlobalCancelledRequestHandler(); // Handle a user-cancelled request.
                               })
                    .AddAHomeController(services, typeof(Startup), "pew pew")
                    .AddACommonJsonFormatter()
                    .AddApiExplorer() // For Swagger/OpenAPI Documentation.
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddProblemDetails(options => options.IncludeExceptionDetails = _ => _hostingEnvironment.IsDevelopment())
                    .AddCustomSwagger("Test API",
                                      SwaggerVersion,
                                      CustomOperationIdSelector);
            
            ConfigureRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            app.UseProblemDetails()
               .UseCustomSwagger("Test API XXX", SwaggerVersion)
               .UseMvc();
        }

        // TODO: Replace this with proper Integration testing overrides.
        //       REF: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.1#basic-tests-with-the-default-webapplicationfactory
        // This method can be overwritten in a test project to define our repository.
        //          You would not do this in production, but we need to test our Helper library!
        public virtual void ConfigureRepositories(IServiceCollection services)
        {
            // Create our fake data.
            var stubbedFakeVehicleRepository = StubbedFakeVehicleRepository.CreateAFakeVehicleRepository();

            // Inject our repo.
            services.AddSingleton<IFakeVehicleRepository>(stubbedFakeVehicleRepository);
        }

        // Format: <Microservice>_<HTTP Method>_<MethodName>
        // E.g. : Search_Head_GetListingsAsync
        private string CustomOperationIdSelector(ApiDescription apiDescription)
        {
            var methodName = apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : Guid.NewGuid().ToString();
            return $"TestMicroservice_{apiDescription.HttpMethod}_{methodName}_{Guid.NewGuid().ToString()}";
        }
    }
}
