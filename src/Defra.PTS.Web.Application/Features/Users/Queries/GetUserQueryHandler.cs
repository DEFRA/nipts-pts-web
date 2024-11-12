using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQueryRequest, UserDetailDto>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserQueryHandler> _logger;

    public GetUserQueryHandler(IUserService userService, ILogger<GetUserQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(logger);

        _userService = userService;
        _logger = logger;
    }

    public async Task<UserDetailDto> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        return await _userService.GetUserDetail(request.UserId);
    }       
}
