using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationDetailsQueryRequest : IRequest<GetApplicationDetailsQueryResponse>
{
    public Guid ApplicationId { get; }
    public GetApplicationDetailsQueryRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
