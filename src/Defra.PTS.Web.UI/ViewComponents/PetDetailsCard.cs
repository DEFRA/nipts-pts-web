using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.ViewComponents;

[ExcludeFromCodeCoverage]
public class PetDetailsCard : ViewComponent
{
    public IViewComponentResult Invoke(string viewName = null)
    {
        var dto = TempData.GetTravelDocument();

        var model = new PetDetailsCvm
        {
            Name = dto.PetName.PetName,
            Species = dto.PetSpecies.PetSpecies.GetDescription(),
            Breed = dto.PetSpecies.PetSpecies == PetSpecies.Ferret ? "-" : dto.PetBreed.BreedName,
            HasBreed = dto.PetSpecies.PetSpecies.HasBreed(),
            Gender = dto.PetGender.Gender.GetDescription(),
            Colour = !string.IsNullOrEmpty(dto.PetColour.PetColourOther) ? dto.PetColour.PetColourOther : dto.PetColour.PetColourName,
            Feature = dto.PetFeature.HasUniqueFeature == YesNoOptions.Yes ? dto.PetFeature.FeatureDescription : "No",
            BirthDate = dto.PetAge.BirthDate.Value.ToUKDateString()
        };

        return View(viewName ?? "Default", model);
    }
}