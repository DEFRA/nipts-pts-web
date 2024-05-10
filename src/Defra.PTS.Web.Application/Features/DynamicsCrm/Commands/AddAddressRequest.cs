using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Domain.Models;
using MediatR;

namespace Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;

public class AddAddressRequest : IRequest<AddAddressResponse>
{
    public User User { get; }
    public AddAddressRequest(User user)
    {
        User = user;
    }
}
