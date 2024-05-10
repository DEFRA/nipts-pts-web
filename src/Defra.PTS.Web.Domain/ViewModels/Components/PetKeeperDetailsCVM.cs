using Defra.PTS.Web.Domain.Models;

namespace Defra.PTS.Web.Domain.ViewModels.Components;

public class PetKeeperDetailsCvm
{
    public string Name { get; set; }

    public string Email { get; set; }

    public Address Address { get; set; }

    public string Phone { get; set; }

    public string NameUrl { get; set; }

    public string PhoneUrl { get; set; }

    public string AddressUrl { get; set; }
}
