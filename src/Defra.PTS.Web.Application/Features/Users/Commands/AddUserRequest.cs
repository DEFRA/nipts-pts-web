using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Domain.Models;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Commands;

public class AddUserRequest(User user) : IRequest<AddUserResponse>
{
    public User User { get; } = user;
}
