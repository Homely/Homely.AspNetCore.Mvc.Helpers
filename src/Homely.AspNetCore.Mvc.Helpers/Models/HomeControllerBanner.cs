using System;
using System.Reflection;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <inheritdoc/>
    public class HomeControllerBanner : IHomeControllerBanner
    {
        private static readonly DateTime ApplicationStartedOn = DateTime.UtcNow;

        public HomeControllerBanner(Type callingType,
                                    string banner = null)
        {
            if (callingType == null)
            {
                throw new ArgumentNullException(nameof(callingType));
            }

            var assembly = callingType.GetTypeInfo().Assembly;
            var assemblyDate = assembly.Location == null
                                   ? "-- unknown --"
                                   : System.IO.File.GetLastWriteTime(assembly.Location).ToString("U");
            var assemblyInfo = $"Name: {assembly.GetName().Name}{Environment.NewLine}" +
                                   $"Version: {assembly.GetName().Version}{Environment.NewLine}" +
                                   $"Build Date : {assemblyDate}{Environment.NewLine}" +
                                   $"Application Started: {ApplicationStartedOn:U}";

            var serverDetails = $"Server name: {Environment.MachineName}";

            Banner = banner + Environment.NewLine + assemblyInfo + Environment.NewLine + serverDetails;
        }

        /// <inheritdoc/> 
        public string Banner { get; }
    }
}