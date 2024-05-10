namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetKeeperPhoneViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"What is your phone number?";

    public string Phone { get; set; }    

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetKeeperPhone;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Phone = default;
    }
}
