using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserDetailQueryRequest : IRequest<GetUserDetailQueryResponse>
{
    public Guid UserId { get; }
    public GetUserDetailQueryRequest(Guid userId)
    {
        UserId = userId;
    }
}
