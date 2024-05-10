using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationsQueryRequest : IRequest<GetApplicationsQueryResponse>
{
    public Guid UserId { get; }
    public List<string> Statuses { get; }
    public GetApplicationsQueryRequest(Guid userId, List<string> statuses)
    {
        UserId = userId;
        Statuses = statuses;    
    }
}
