namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperNameViewModel : TravelDocumentFormPage
{
    public static string FormTitle => $"What is your full name?";

    public string Name { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperName;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Name = default;
    }
}
