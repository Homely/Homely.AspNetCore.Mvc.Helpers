//using Homely.AspNetCore.Mvc.Helpers.Extensions;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using System;
//using TestWebApplication;

//namespace Homely.AspNetCore.Mvc.Helpers.Tests
//{
//    public class TestStartupWithCustomJsonExceptionPageIncludeStackTrace : Startup
//    {
//        public TestStartupWithCustomJsonExceptionPageIncludeStackTrace(IConfiguration configuration) : base(configuration)
//        {
//        }

//        public override void ConfigureJsonExceptionPages(IApplicationBuilder app)
//        {
//            if (app == null)
//            {
//                throw new ArgumentNullException(nameof(app));
//            }
            
//            app.UseJsonExceptionPages(includeStackTrace: true);
//        }
//    }
//}
