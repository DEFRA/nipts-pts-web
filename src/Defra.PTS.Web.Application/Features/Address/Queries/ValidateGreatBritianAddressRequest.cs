using MediatR;

namespace Defra.PTS.Web.Application.Features.Address.Queries;

public class ValidateGreatBritianAddressRequest : IRequest<bool>
{
    public string Postcode { get; }
    public ValidateGreatBritianAddressRequest(string postcode)
    {
        Postcode = postcode;
    }
}
