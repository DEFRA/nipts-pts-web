using Defra.PTS.Web.CertificateGenerator.Models;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Certificates.Commands;


public class GenerateApplicationPdfRequest : IRequest<CertificateResult>
{
    public Guid ApplicationId { get; }
    public GenerateApplicationPdfRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
