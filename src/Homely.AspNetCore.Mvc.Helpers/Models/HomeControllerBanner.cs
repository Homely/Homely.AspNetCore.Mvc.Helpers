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
                                   : System.IO.File.GetLastWriteTime(assembly.Location).ToString("dd-MMMM-yyyy hh:mm:ss");
            var assemblyInfo = $"Name: {assembly.GetName().Name}{Environment.NewLine}" +
                                   $"Version: {assembly.GetName().Version}{Environment.NewLine}" +
                                   $"Date: {assemblyDate}";

            Banner = banner + Environment.NewLine + assemblyInfo;
        }

        public string Banner { get; }
    }
}