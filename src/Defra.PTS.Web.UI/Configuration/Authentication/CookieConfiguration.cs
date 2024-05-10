using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Configuration.Authentication
{
    [ExcludeFromCodeCoverage]
    public class CookieConfiguration
    {
        public TimeSpan ExpireTimespan { get; set; }
        public string Name { get; set; }
    }
}
