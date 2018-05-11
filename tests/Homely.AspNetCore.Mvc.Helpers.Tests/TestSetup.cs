using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public abstract class TestSetup
    {
        public TestSetup()
        {
            TestServer = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            Client = TestServer.CreateClient();
        }

        public TestServer TestServer { get; set; }

        public HttpClient Client { get; }        
    }
}
