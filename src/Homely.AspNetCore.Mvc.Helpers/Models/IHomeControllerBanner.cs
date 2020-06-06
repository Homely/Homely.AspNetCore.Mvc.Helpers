namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <summary>
    /// Homepage controller banner - which displays any provided ASCII art + assembly info.
    /// </summary>
    /// <remarks>The assembly info is a great option to visually confirm the current build assembly/dll information.</remarks>
    public interface IHomeControllerBanner
    {
        /// <summary>
        /// Text / ASCII Banner   ۜ\(סּںסּَ` )/ۜ
        /// </summary>
        string Banner { get; }
    }
}