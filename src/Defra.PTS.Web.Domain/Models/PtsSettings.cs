using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class PtsSettings
    {
        public bool MagicWordEnabled { get; set; }
        public string MagicWord { get; set; }

    }
}
