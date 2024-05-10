using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Address.Queries;

public class ValidateGreatBritianAddressHandler : IRequestHandler<ValidateGreatBritianAddressRequest, bool>
{
    private readonly IAddressLookupService _addressLookupService;
    private readonly ILogger<ValidateGreatBritianAddressHandler> _logger;

    public ValidateGreatBritianAddressHandler(IAddressLookupService addressLookupService, ILogger<ValidateGreatBritianAddressHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(addressLookupService);
        ArgumentNullException.ThrowIfNull(logger);

        _addressLookupService = addressLookupService;
        _logger = logger;
    }

    public async Task<bool> Handle(ValidateGreatBritianAddressRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await IsGbAddress(request.Postcode);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(_addressLookupService)}: Unable to get validadate Gb address for {request.Postcode}", ex);
            return false;
        }
    }

    private async Task<bool> IsGbAddress(string postcode)
    {
        // No postcode
        if (string.IsNullOrWhiteSpace(postcode))
        {
            return false;
        }

        // Non GB Postcodes based on first 2 letters
        if (ApplicationHelper.PostcodeStartsWithNonGBPrefix(postcode))
        {
            return false;
        }

        try
        {
            var response = new AddressLookupResponse
            {
                Postcode = postcode,
                Addresses = await _addressLookupService.FindAddressesByPostcode(postcode)
            };

            var isGBAddress = response.Addresses != null && response.Addresses.Count > 0;

            return isGBAddress;
        }
        catch (Exception ex)
        {
            _logger.LogError("Non GB address inserted in IDM2", ex);

            return false;
        }
    }
}
