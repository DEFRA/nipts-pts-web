using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetIsUserSuspendedQueryHandler : IRequestHandler<GetIsUserSuspendedQueryRequest, GetIsUserSuspendedQueryResponse>
{
    private readonly IUserService _userService;
    public GetIsUserSuspendedQueryHandler(IUserService userService)
    {
        ArgumentNullException.ThrowIfNull(userService);

        _userService = userService;
    }

    public async Task<GetIsUserSuspendedQueryResponse> Handle(GetIsUserSuspendedQueryRequest request, CancellationToken cancellationToken)
    {
        return new GetIsUserSuspendedQueryResponse
        {
            IsSuspended = await _userService.IsUserSuspended(request.ContactId),
        };
    }
}
