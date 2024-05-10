using Defra.PTS.Web.Application.DTOs.Services;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserQueryRequest : IRequest<UserDetailDto>
{
    public Guid UserId { get; }    
    public GetUserQueryRequest(Guid userId)
    {
        UserId = userId;
    }
}
