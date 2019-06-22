using System;
using System.Reflection;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <summary>
    /// Homepage controller banner - which displays any provided ASCII art + assembly info.
    /// </summary>
    /// <remarks>The assembly info is a great option to visually confirm the current build assembly/dll information.</remarks>
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
                                   $"Application Started: {ApplicationStartedOn.ToString("U")}";

            Banner = banner + Environment.NewLine + assemblyInfo;
        }

        public string Banner { get; }
    }
}