using System;
using System.Reflection;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
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