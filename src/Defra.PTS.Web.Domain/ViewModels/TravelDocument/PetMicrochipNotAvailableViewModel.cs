using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetMicrochipNotAvailableViewModel : TravelDocumentFormPage
{
    public static string FormTitle => $"Get your pet microchipped before applying";

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetMicrochipNotAvailable;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
    }
}
