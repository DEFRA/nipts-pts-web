using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetSpeciesViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"Is your pet a dog, cat or ferret?";

    public PetSpecies PetSpecies { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetSpecies;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        PetSpecies = default;
    }

}
