using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.ViewComponents;

[ExcludeFromCodeCoverage]
public class MicrochipInformationCard : ViewComponent
{
    public IViewComponentResult Invoke(string viewName)
    {
        var dto = TempData.GetTravelDocument();

        var model = new MicrochipInformationCvm
        {
            MicrochipNumber = dto.PetMicrochip?.MicrochipNumber ?? string.Empty,
            MicrochipDate = dto.PetMicrochipDate.MicrochippedDate.HasValue ? dto.PetMicrochipDate?.MicrochippedDate.Value.ToUKDateString() : string.Empty,
            // we need to display the microchip location as "Under the skin" on the pet travel document, even though we don't ask the user the question.
            // This is from Policy as it is a requirement for the field to be on the pet travel document.
            MicrochipImplantLocation = "Under the skin"
        };

        return View(viewName, model);
    }
}
