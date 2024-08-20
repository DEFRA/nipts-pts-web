using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperUserDetailsViewModel : TravelDocumentFormPage, IPostcodeModel
{
    public string PetKeeperUserDetailsFormTitle => $"Are your details correct?";
    public string PetKeeperNonGbAddressFormTitle => $"Change your details";
    public string PetKeeperNonGbAddressFormMessageHeader => $"Important";
    public string PetKeeperNonGbAddressFormMessageBody => $"Enter an address in England, Scotland or Wales.";

    public YesNoOptions UserDetailsAreCorrect { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string AddressLineOne { get; set; }

    public string TownOrCity { get; set; }

    public string County { get; set; }

    public string Postcode { get; set; }

    public PostcodeRegion PostcodeRegion { get; set; } = PostcodeRegion.Unknown;

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperUserDetails;

    private bool _petOwnerDetailsRequired;
    public bool PetOwnerDetailsRequired
    {
        get
        {
            if(UserDetailsAreCorrect == YesNoOptions.No)
                _petOwnerDetailsRequired = true;
            
            return _petOwnerDetailsRequired;
        }
        set
        {
            _petOwnerDetailsRequired = value;
        }
    }

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Name = default;
        Phone = default;
        Email = default;
        AddressLineOne = default;
        TownOrCity = default;
        County = default;
        Postcode = default;
    }
}
