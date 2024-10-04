using Defra.PTS.Web.Domain.Models;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class AddressLookupResponse
{
    public string Postcode { get; set; }
    public List<Address> Addresses { get; set; } = [];
}
