using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationsQueryRequest(Guid userId, List<string> statuses) : IRequest<GetApplicationsQueryResponse>
{
    public Guid UserId { get; } = userId;
    public List<string> Statuses { get; } = statuses;
}
