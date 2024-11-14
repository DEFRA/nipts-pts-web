using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Constants;

[ExcludeFromCodeCoverage]
public class GoogleTagManager
{
    public string ContainerId { get; set; } = string.Empty;
    public string MeasurementId { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
}
