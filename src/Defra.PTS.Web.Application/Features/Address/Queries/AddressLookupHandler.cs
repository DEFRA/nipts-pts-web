using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Address.Queries;

public class AddressLookupHandler : IRequestHandler<AddressLookupRequest, AddressLookupResponse>
{
    private readonly IAddressLookupService _addressLookupService;
    private readonly ILogger<AddressLookupHandler> _logger;

    public AddressLookupHandler(IAddressLookupService addressLookupService, ILogger<AddressLookupHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(addressLookupService);
        ArgumentNullException.ThrowIfNull(logger);

        _addressLookupService = addressLookupService;
        _logger = logger;
    }

    public async Task<AddressLookupResponse> Handle(AddressLookupRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new AddressLookupResponse
            {
                Postcode = request.Postcode,
                Addresses = await _addressLookupService.FindAddressesByPostcode(request.Postcode)
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(_addressLookupService)}: Unable to get list of addresses for {request.Postcode}", ex);
            throw;
        }
    }
}
