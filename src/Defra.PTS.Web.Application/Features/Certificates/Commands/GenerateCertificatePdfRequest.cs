using Defra.PTS.Web.CertificateGenerator.Models;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Certificates.Commands;

public class GenerateCertificatePdfRequest : IRequest<CertificateResult>
{
    public Guid ApplicationId { get; }
    public GenerateCertificatePdfRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
