using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public string UserServiceUrl { get; set; }

        public string PetServiceUrl { get; set; }

        public string ApplicationServiceUrl { get; set; }

        public string DynamicServiceUrl { get; set; }

        public bool UseMockCs { get; set; }

        public string ServiceUrl { get; set; }
    }
}
