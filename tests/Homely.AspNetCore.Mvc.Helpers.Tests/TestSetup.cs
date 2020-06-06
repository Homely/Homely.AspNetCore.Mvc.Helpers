using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using TestWebApplication;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public abstract class TestSetup : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;

        protected TestSetup()
        {
            _factory = new WebApplicationFactory<Startup>();

            //TestServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            Client = _factory.CreateClient();
        }

        //public TestServer TestServer { get; set; }

        public HttpClient Client { get; }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
