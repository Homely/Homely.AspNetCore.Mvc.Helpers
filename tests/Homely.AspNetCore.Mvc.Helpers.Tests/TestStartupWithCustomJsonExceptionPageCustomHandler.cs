//using Homely.AspNetCore.Mvc.Helpers.Extensions;
//using Homely.AspNetCore.Mvc.Helpers.Models;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using TestWebApplication;

//namespace Homely.AspNetCore.Mvc.Helpers.Tests
//{
//    public class TestStartupWithCustomJsonExceptionPageCustomHandler : Startup
//    {
//        public TestStartupWithCustomJsonExceptionPageCustomHandler(IConfiguration configuration) : base(configuration)
//        {
//        }

//        public override void ConfigureJsonExceptionPages(IApplicationBuilder app)
//        {
//            if (app == null)
//            {
//                throw new ArgumentNullException(nameof(app));
//            }

//            JsonExceptionPageResult HandleException(Exception result)
//            {
//                return new JsonExceptionPageResult
//                {
//                    StatusCode = System.Net.HttpStatusCode.UpgradeRequired,
//                    ApiErrors = new List<ApiError>
//                    {
//                        new ApiError
//                        {
//                            Message = "I'm a little tea pot, short and stout."
//                        }
//                    }
//                };
//            }

//            app.UseJsonExceptionPages(customExceptionFunction: HandleException);
//        }
//    }
//}
