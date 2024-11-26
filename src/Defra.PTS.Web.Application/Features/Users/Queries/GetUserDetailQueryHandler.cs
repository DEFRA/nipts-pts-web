using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQueryRequest, GetUserDetailQueryResponse>
{
    private readonly IUserService _userService;
    public GetUserDetailQueryHandler(IUserService userService)
    {
        ArgumentNullException.ThrowIfNull(userService);

        _userService = userService;
    }

    public async Task<GetUserDetailQueryResponse> Handle(GetUserDetailQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetUserDetailQueryResponse
        {
            UserId = request.UserId,
            UserDetail = await _userService.GetUserDetail(request.UserId),
        };
        return response;
    }

}

