using Defra.PTS.Web.Domain.Models;

namespace Defra.PTS.Web.Infrastructure.Services.Interfaces;

public interface IAddressLookupService
{
    Task<List<Address>> FindAddressesByPostcode(string postcode);
}
