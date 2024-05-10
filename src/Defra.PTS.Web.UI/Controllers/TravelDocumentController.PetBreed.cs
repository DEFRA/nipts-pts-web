using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> PetBreed()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetSpecies);

        var formData = GetFormData();

        if (!formData.DoesPageMeetPreConditions(formData.PetBreed.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        formData.PetBreed.PetSpecies = formData.PetSpecies.PetSpecies;
        SaveFormData(formData.PetBreed);

        if (formData.PetSpecies.PetSpecies.HasBreed())
        {
            var breeds = await GetBreedsAsSelectListItems(formData.PetBreed.PetSpecies);
            ViewBag.BreedList = breeds;
            if (formData.PetBreed.BreedId > 0 && formData.PetBreed.BreedName != null)
            {
                var breed = breeds.Find(x => x.Text == formData.PetBreed.BreedName);
                if(breed == null)
                {
                    formData.PetBreed.BreedId = 0;
                }

                if (formData.PetBreed.BreedAdditionalInfo != null)
                {
                    ViewBag.BreedList.Add(new SelectListItem() { Value = "0", Text = formData.PetBreed.BreedAdditionalInfo, Selected = true });
                }
            }
        }

        return View(formData.PetBreed);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PetBreed(PetBreedViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetSpecies);

        List<SelectListItem> breeds = null;

        if (model.PetSpecies.HasBreed())
        {
            breeds = await GetBreedsAsSelectListItems(model.PetSpecies);
            ViewBag.BreedList = breeds;           
            
            //BreedId = 0 (freetext, first entry), 98 (Cat, Mixed or Other), 99 (Dog, Mixed or Other)
            //We need to sort freetext entries to see if they match an item on our list first
            //If it is not found, we assign it as a mixed breed
            if (model.BreedId == 0 || model.BreedId == 98 || model.BreedId == 99)
            {
                if (!ModelState.IsValid)
                {
                    if (model.BreedName != null)
                    {
                        ViewBag.BreedList.Add(new SelectListItem() { Value = "0", Text = model.BreedName, Selected = true });
                    }
                    return View(model);
                }

                var normalisedBreed = model.BreedName?.Trim();
                var intendedBreed = breeds.Find(x => x.Text.ToLower() == normalisedBreed?.ToLower());

                if (intendedBreed != null)
                {
                    model.BreedId = Convert.ToInt32(intendedBreed.Value.ToString());
                    model.BreedName = intendedBreed.Text;
                    model.BreedAdditionalInfo = null;
                }

                else
                {
                    var breed = breeds.Find(x => x.Text == "Mixed breed or unknown");
                    model.BreedId = Convert.ToInt32(breed?.Value.ToString());
                    model.BreedAdditionalInfo = model?.BreedName;
                }
            }
            else
            {
                var breed = breeds.Find(x => x.Text == model.BreedName);
                if (breed == null)
                {
                    if (model.PetSpecies == Domain.Enums.PetSpecies.Dog)
                    {
                        model.BreedId = 99;
                        model.BreedAdditionalInfo = model.BreedName;
                    }
                    else if (model.PetSpecies == Domain.Enums.PetSpecies.Cat)
                    {
                        model.BreedId = 98;
                        model.BreedAdditionalInfo = model.BreedName;
                    }
                    else
                    {
                        model.BreedId = -1;
                        model.BreedName = null;
                        model.BreedAdditionalInfo = null;
                    }
                }
                else
                {
                    model.BreedId = Int32.Parse(breed.Value);
                    model.BreedAdditionalInfo = null;
                }
            }
        }
        else
        {
            model.BreedName = string.Empty;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.BreedList = breeds;            
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetName));
    }

    #region Private Methods
    private async Task<List<SelectListItem>> GetBreedsAsSelectListItems(PetSpecies petType)
    {
        var response = await _mediator.Send(new GetBreedsQueryRequest(petType));
        return response.Breeds.ToSelectListItems();
    }

    #endregion Private Methods
}
