using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetMicrochipViewModel : TravelDocumentFormPage
{
    public static string FormTitle => $"Is your pet microchipped?";

    public YesNoOptions Microchipped { get; set; }

    public string MicrochipNumber { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetMicrochip;

    public override void TrimUnwantedData()
    {
        switch (Microchipped)
        {
            case YesNoOptions.Yes:
                if (!string.IsNullOrWhiteSpace(MicrochipNumber))
                {
                    MicrochipNumber = MicrochipNumber.Trim().Replace(" ", string.Empty);
                }
                else
                {
                    MicrochipNumber = string.Empty;
                }
                break;
            default:
                MicrochipNumber = string.Empty;
                break;
        }
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Microchipped = default;
        MicrochipNumber = default;
    }
}
