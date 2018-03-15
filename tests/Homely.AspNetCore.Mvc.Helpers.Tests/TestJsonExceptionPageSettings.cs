using Homely.AspNetCore.Mvc.Helpers.Models;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class TestJsonExceptionPageSettings
    {
        public bool IncludeStackTrace { get; set;}
        public Func<Exception, JsonExceptionPageResult> CustomExceptionFunction { get; set; }
    }
}
