using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> PetColour()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetColour.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetAge);

        var colours = await GetColoursList(formData.PetSpecies.PetSpecies);
        ViewBag.Colours = colours;

        formData.PetColour.PetSpecies = formData.PetSpecies.PetSpecies;
        formData.PetColour.OtherColourID = GetOtherColourId(colours);
        SaveFormData(formData.PetColour);

        return View(formData.PetColour);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PetColour(PetColourViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetAge);
        var colours = await GetColoursList(model.PetSpecies);

        if (!ModelState.IsValid)
        {
            ViewBag.Colours = colours;
            return View(model);
        }

        model.TrimUnwantedData();
        model.PetColourName = colours.First(x => int.Parse(x.Id) == model.PetColour).Name;
        model.IsCompleted = true;

        SaveFormData(model);

        return RedirectToAction(nameof(PetFeature));
    }

    private async Task<List<ColourDto>> GetColoursList(PetSpecies petSpecies)
    {
        var colourResponse = await _mediator.Send(new GetColoursQueryRequest(petSpecies));
        if (colourResponse == null || colourResponse.Colours == null || !colourResponse.Colours.Any())
        {
            return new List<ColourDto>();
        }

        var otherColour = colourResponse.Colours?.FirstOrDefault(x => x.Name.ToLowerInvariant() == AppConstants.Values.OtherColourName.ToLowerInvariant());
        if (otherColour != null)
        {
            otherColour.DisplayOrder = int.MaxValue;
        }

        // Order by DisplayOrder, then by Name
        var orderedColours = colourResponse.Colours
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToList();

        return orderedColours;
    }

    private static int GetOtherColourId(List<ColourDto> colours)
    {
        var otherColour = colours?.FirstOrDefault(x => x.Name.ToLowerInvariant() == AppConstants.Values.OtherColourName.ToLowerInvariant());
        if (otherColour == null)
        {
            return default;
        }

        return int.Parse(otherColour.Id);
    }
}
