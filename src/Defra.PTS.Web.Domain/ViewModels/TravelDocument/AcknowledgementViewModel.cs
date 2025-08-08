using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class AcknowledgementViewModel : TravelDocumentFormPage
{
    public static string FormTitle => $"Application submitted";

    public string Reference { get; set; }

    public bool IsSuccess { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.Acknowledgement;

    public override void ClearData()
    {
        IsCompleted = default;
        Reference = default;
        IsSuccess = default;
    }

    public override void TrimUnwantedData()
    {
    }
}