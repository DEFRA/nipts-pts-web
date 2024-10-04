using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationCertificateQueryRequest(Guid applicationId) : IRequest<GetApplicationCertificateQueryResponse>
{
    public Guid ApplicationId { get; } = applicationId;
}
