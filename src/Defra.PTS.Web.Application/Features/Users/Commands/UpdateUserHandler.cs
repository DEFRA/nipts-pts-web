using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Users.Commands;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService, ILogger<UpdateUserHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);

        _userService = userService;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var response = new UpdateUserResponse
        {
            IsSuccess = true,
            UserId = await _userService.UpdateUserAsync(request.EmailAddress)
        };

        return response;
    }
}
