using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQueryRequest, GetUserDetailQueryResponse>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserDetailQueryHandler> _logger;
    public GetUserDetailQueryHandler(IUserService userService, ILogger<GetUserDetailQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(logger);

        _userService = userService;
        _logger = logger;
    }

    public async Task<GetUserDetailQueryResponse> Handle(GetUserDetailQueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetUserDetailQueryResponse
            {
                UserId = request.UserId,
                UserDetail = await _userService.GetUserDetail(request.UserId),
            };
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(_userService)}: Unable to  UserDetail {request?.UserId}", ex);
            throw;
        }
    }

}

