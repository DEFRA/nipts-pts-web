using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class DeclarationViewModel : TravelDocumentFormPage
{

    public static string FormTitle => $"Check your answers and sign the declaration";

    public bool AgreedToAccuracy { get; set; }

    public bool AgreedToPrivacyPolicy { get; set; }

    public bool AgreedToDeclaration { get; set; }

    public Guid RequestId { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.Declaration;

    public string Phone { get; set; }
    public string Postcode { get; set; }
    public bool IsManualAddress { get; set; }

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        AgreedToAccuracy = default;
        AgreedToPrivacyPolicy = default;
        AgreedToDeclaration = default;
    }

}
