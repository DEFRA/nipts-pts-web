using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Client;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class AddressLookupService : IAddressLookupService
{
    private readonly IPlacesApi _placesApi;

    public AddressLookupService(IPlacesApi placesApi)
    {
        ArgumentNullException.ThrowIfNull(placesApi);

        _placesApi = placesApi;
    }

    public async Task<List<Address>> FindAddressesByPostcode(string postcode)
    {
        try
        {
            var addresses = await _placesApi.PostCodeLookupAsync(postcode);

            return addresses.Select(x => ConvertToAddress(x)).ToList();
        }

        catch (ApiException)
        {
            // this is for where the api has no values returned for a valid postcode entry
            return new List<Address>();
        }
    }

    private static Address ConvertToAddress(Trade.Address.V1.ApiClient.Model.AddressDto addressDto)
    {
        var address = new Address();

        var values = addressDto.Address.Split(',');
        if (values.Length > 0)
        {
            address.AddressLineOne = values[0];
        }

        if (values.Length > 1)
        {
            address.AddressLineOne = $"{values[0]} {values[1]}";
        }

        if (values.Length > 2)
        {
            address.AddressLineTwo = values[2];
        }

        address.TownOrCity = addressDto.PostTown;
        address.Postcode = addressDto.Postcode;

        return address;
    }

}
