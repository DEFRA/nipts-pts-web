using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Commands;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUserService _userService;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUserService userService, ILogger<UpdateUserHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(logger);

        _userService = userService;
        _logger = logger;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new UpdateUserResponse
            {
                IsSuccess = true,
                UserId = await _userService.UpdateUserAsync(request.EmailAddress)
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(_userService)}: Unable to update user {request?.EmailAddress}");
            throw;
        }
    }
}
