using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Address.Queries;

public class AddressLookupRequest : IRequest<AddressLookupResponse>
{
    public string Postcode { get; }
    public AddressLookupRequest(string postcode)
    {
        Postcode = postcode;
    }
}
