using Hellang.Middleware.ProblemDetails;
using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using TestWebApplication.Repositories;

namespace TestWebApplication
{
    public class Startup
    {
        private const string SwaggerVersion = "v2";
        private const string SwaggerTitle = "Test API";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddAHomeController(services, typeof(Startup), "pew pew")
                    .AddDefaultJsonOptions();

            // NOTE: For this test web application, we don't need a TRACE ID because it messes with our tests
            //       when we need to check the test results.
            services.AddProblemDetails(options =>
                                      {
                                          options.IncludeExceptionDetails = (ctx, ex) => _webHostEnvironment.IsDevelopment();
                                          options.GetTraceId = new Func<Microsoft.AspNetCore.Http.HttpContext, string>(httpContext => null);
                                      })
                    .AddCustomSwagger(SwaggerTitle,
                                      SwaggerVersion,
                                      CustomOperationIdSelector);
            
            ConfigureRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails()
               .UseCustomSwagger(SwaggerTitle, SwaggerVersion)
               .UseRouting()
               .UseEndpoints(endpoints => endpoints.MapControllers());
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
        // E.g. : TestMicroservice_Head_GetListingsAsync
        private string CustomOperationIdSelector(ApiDescription apiDescription)
        {
            var methodName = apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : Guid.NewGuid().ToString();
            return $"TestMicroservice_{apiDescription.HttpMethod}_{methodName}_{Guid.NewGuid().ToString()}";
        }
    }
}
