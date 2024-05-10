using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperAddressManualViewModel : TravelDocumentFormPage, IPostcodeModel
{
    public string FormTitle => $"What is your address?";

    public string AddressLineOne { get; set; }

    public string AddressLineTwo { get; set; }

    public string TownOrCity { get; set; }

    public string County { get; set; }

    public string Postcode { get; set; }

    public PostcodeRegion PostcodeRegion { get; set; } = PostcodeRegion.Unknown; 

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperAddressManual;


    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        AddressLineOne = default;
        AddressLineTwo = default;
        TownOrCity = default;
        County = default;
        Postcode = default;
    }

}
