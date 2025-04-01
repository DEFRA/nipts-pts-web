using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Infrastructure.Models;

[ExcludeFromCodeCoverage]
public class ApplicationSubmittedMessage
{
    public Guid ApplicationId { get; set; }
    public int ApplicationLanguage { get; set; }
}
