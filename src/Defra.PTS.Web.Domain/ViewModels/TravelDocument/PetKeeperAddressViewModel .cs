using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperAddressViewModel : TravelDocumentFormPage, IPostcodeModel
{
    public static string FormTitle => $"What is your address?";

    public string Address { get; set; }

    public string Postcode { get; set; }

    public PostcodeRegion PostcodeRegion { get; set; } = PostcodeRegion.Unknown;

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperAddress;

    public override void TrimUnwantedData()
    {
    }
    public override void ClearData()
    {
        IsCompleted = default;
        Address = default;
        Postcode = default;
    }
}
