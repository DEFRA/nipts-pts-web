using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Commands;

public class UpdateUserRequest(string emailAddress) : IRequest<UpdateUserResponse>
{
    public string EmailAddress { get; } = emailAddress;
}
