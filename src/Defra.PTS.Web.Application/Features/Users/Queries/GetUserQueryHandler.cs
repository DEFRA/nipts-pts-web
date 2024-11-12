using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQueryRequest, UserDetailDto>
{
    private readonly IUserService _userService;

    public GetUserQueryHandler(IUserService userService)
    {
        ArgumentNullException.ThrowIfNull(userService);

        _userService = userService;
    }

    public async Task<UserDetailDto> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        return await _userService.GetUserDetail(request.UserId);
    }       
}
