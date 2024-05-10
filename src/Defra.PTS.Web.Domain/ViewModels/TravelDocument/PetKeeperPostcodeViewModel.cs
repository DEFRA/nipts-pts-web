using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperPostcodeViewModel : TravelDocumentFormPage, IPostcodeModel
{
    public string FormTitle => $"What is your postcode?";

    public string Postcode { get; set; }

    public PostcodeRegion PostcodeRegion { get; set; } = PostcodeRegion.Unknown;

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperPostcode;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Postcode = default;
    }
}
