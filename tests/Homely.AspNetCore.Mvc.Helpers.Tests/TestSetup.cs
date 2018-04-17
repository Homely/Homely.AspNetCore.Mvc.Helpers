using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public abstract class TestSetup
    {
        private readonly HttpClient _client;

        public TestSetup()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = server.CreateClient();
        }

        public HttpClient Client => _client;
    }
}
