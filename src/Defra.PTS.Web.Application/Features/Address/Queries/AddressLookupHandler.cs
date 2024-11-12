using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Address.Queries;

public class AddressLookupHandler : IRequestHandler<AddressLookupRequest, AddressLookupResponse>
{
    private readonly IAddressLookupService _addressLookupService;

    public AddressLookupHandler(IAddressLookupService addressLookupService)
    {
        ArgumentNullException.ThrowIfNull(addressLookupService);

        _addressLookupService = addressLookupService;
    }

    public async Task<AddressLookupResponse> Handle(AddressLookupRequest request, CancellationToken cancellationToken)
    {
        var response = new AddressLookupResponse
        {
            Postcode = request.Postcode,
            Addresses = await _addressLookupService.FindAddressesByPostcode(request.Postcode)
        };

        return response;
    }
}
