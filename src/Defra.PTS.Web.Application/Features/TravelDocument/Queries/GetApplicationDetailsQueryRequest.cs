using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationDetailsQueryRequest(Guid applicationId) : IRequest<GetApplicationDetailsQueryResponse>
{
    public Guid ApplicationId { get; } = applicationId;
}
