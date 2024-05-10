using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetApplicationCertificateQueryResponse
{
    public Guid ApplicationId { get; set; }
    public ApplicationCertificateDto ApplicationCertificate { get; set; } = new();
}
