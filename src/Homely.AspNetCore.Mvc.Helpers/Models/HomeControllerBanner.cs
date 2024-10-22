using System;
using System.Reflection;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <inheritdoc/>
    public class HomeControllerBanner : IHomeControllerBanner
    {
        private static readonly DateTime ApplicationStartedOn = DateTime.Now;

        public HomeControllerBanner(Assembly callingAssembly, string? banner = null)
        {
            var assemblyDate = callingAssembly.Location == null
                                   ? "-- unknown --"
                                   : System.IO.File.GetLastWriteTime(callingAssembly.Location).ToString("F");

            var assemblyInfo = $"Name: {callingAssembly.GetName().Name}{Environment.NewLine}" +
                                   $"Version: {callingAssembly.GetName().Version}{Environment.NewLine}" +
                                   $"Build Date : {assemblyDate}{Environment.NewLine}" +
                                   $"Application Started: {ApplicationStartedOn:F}";

            var serverDetails = $"Server name: {Environment.MachineName}";

            Banner = banner + Environment.NewLine + assemblyInfo + Environment.NewLine + serverDetails;
        }

        /// <inheritdoc/> 
        public string Banner { get; }
    }
}
