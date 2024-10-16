using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserDetailQueryRequest(Guid userId) : IRequest<GetUserDetailQueryResponse>
{
    public Guid UserId { get; } = userId;
}
