using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationCertificateQueryRequest : IRequest<GetApplicationCertificateQueryResponse>
{
    public Guid ApplicationId { get; }
    public GetApplicationCertificateQueryRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
