using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Commands;

public class AddUserHandler : IRequestHandler<AddUserRequest, AddUserResponse>
{
    private readonly IUserService _userService;
    private readonly ILogger<AddUserHandler> _logger;

    public AddUserHandler(IUserService userService, ILogger<AddUserHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(logger);

        _userService = userService;
        _logger = logger;
    }

    public async Task<AddUserResponse> Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new AddUserResponse
            {
                IsSuccess = true,
                UserId = await _userService.AddUserAsync(request.User)
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{userService}: Unable to add user {UniqueReference}", nameof(_userService), request?.User?.UniqueReference);
            throw;
        }
    }
}
