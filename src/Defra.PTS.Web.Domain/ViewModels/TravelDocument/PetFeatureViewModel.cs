using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetFeatureViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"Does your pet have any significant features?";

    public YesNoOptions HasUniqueFeature { get; set; }

    public string FeatureDescription { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetFeature;

    public override void TrimUnwantedData()
    {
        switch (HasUniqueFeature)
        {
            case YesNoOptions.No:
                FeatureDescription = string.Empty;
                break;
            default:
                break;
        }
    }
    public override void ClearData()
    {
        IsCompleted = default;
        HasUniqueFeature = default;
        FeatureDescription = default;
    }
}
