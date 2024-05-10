using Defra.PTS.Web.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetBreedViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"What breed is your {PetTypeNameLowered}?";

    public PetSpecies PetSpecies { get; set; }

    [StringLength(150, ErrorMessage = "The breed name must be less than or equal to 150 characters.")]
    public string BreedName { get; set; }    

    public int BreedId { get; set; }
    public string BreedAdditionalInfo { get; set; }

    public string PetTypeNameLowered => Enum.GetName(typeof(PetSpecies), PetSpecies).ToLower();

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetBreed;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        PetSpecies = default;
        BreedName = default;
        BreedId = default;
    }
}
