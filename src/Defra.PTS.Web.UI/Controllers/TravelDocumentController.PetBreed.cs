using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Defra.PTS.Web.UI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Localization;

using System.Drawing.Drawing2D;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> PetBreed()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
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

            //we need to check if the user is using Welsh - if they are, we change the order 
            if (Thread.CurrentThread.CurrentCulture.EnglishName.Contains("Welsh"))
            {
                ViewBag.BreedList = breeds
                .OrderBy(i => i.Value != "0" && i.Value != "99" && i.Value != "100")
                .ThenBy(i => i.Text, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
            }


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

        List<SelectListItem> breeds = await GetBreedsAsSelectListItems(model.PetSpecies);

        var formData = GetFormData();

        if (model.BreedId != 0 && ModelState.HasError(nameof(model.BreedName)))
        {
            // Correct and set the BreedName if possible
            model.BreedName = breeds.Find(b => b.Value == model.BreedId.ToString())?.Text;

            // Clear the existing validation state for BreedName
            ModelState.Remove(nameof(model.BreedName));

            // Re-validate model
            TryValidateModel(model);
        }

        if (!ModelState.IsValid)
        {
            ViewBag.BreedList = await GetBreedsAsSelectListItems(model.PetSpecies);
            return View(model);
        }

        ViewBag.BreedList = breeds;

        //If typed value is not in breedList (matched by BreedName) set ID to 0
        var typedBreed = model.BreedName?.Trim(); 
        var compareBreed = breeds.Find(x => x.Text.Equals(typedBreed?.ToLower(), StringComparison.CurrentCultureIgnoreCase));

        if (compareBreed == null)
        {
            // If the breed is NOT in the list, assign ID to 0
            model.BreedId = 0; 
            model.BreedName = typedBreed;
            model.BreedAdditionalInfo = null;
        }

        await AssignBreed(model);

        if (!ModelState.IsValid)
        {
            ViewBag.BreedList = breeds;            
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        if (GetCYACheck() && formData.PetColour.PetColour == 0)
        {
            return RedirectToAction(nameof(PetColour));
        }
        return CYARedirect(nameof(PetName));
    }

    #region Private Methods

    private async Task<List<SelectListItem>> GetBreedsAsSelectListItems(PetSpecies petType)
    {
        var list = await _selectListLocaliser.GetBreedList(petType);

        // Order by Name
        var orderedColours = list
            //Mixed Breed or unknown to always be at top of list
            .OrderBy(x => x.BreedName.StartsWith(_localizer["Mixed breed"]) ? 0 : 1)
            // If free text or BreedName null then don't sort
            .ThenBy(x => x.BreedName ?? string.Empty)
            .ToList();

        return orderedColours.ToSelectListItems();
    }

    private async Task AssignBreed(PetBreedViewModel model)
    {
        List<SelectListItem> breeds = null;
        breeds = await GetBreedsAsSelectListItems(model.PetSpecies);

        if (model.BreedId == 0 || model.BreedId == 99 || model.BreedId == 100)
        {
            var normalisedBreed = model.BreedName?.Trim();
            var intendedBreed = breeds.Find(x => x.Text.Equals(normalisedBreed?.ToLower(), StringComparison.CurrentCultureIgnoreCase));

            if (intendedBreed != null)
            {
                model.BreedId = Convert.ToInt32(intendedBreed.Value.ToString());
                model.BreedName = intendedBreed.Text;
                model.BreedAdditionalInfo = null;
            }

            else
            {
                if (model.PetSpecies == Domain.Enums.PetSpecies.Dog)
                {
                    model.BreedId = 99;
                    model.BreedAdditionalInfo = model.BreedName;
                }
                else if (model.PetSpecies == Domain.Enums.PetSpecies.Cat)
                {
                    model.BreedId = 100;
                    model.BreedAdditionalInfo = model.BreedName;
                }
            }
        }
        else
        {
            var breed = breeds.Find(x => x.Value == model.BreedId.ToString());
            model.BreedId = Int32.Parse(breed.Value);
            model.BreedName = breed.Text;
            model.BreedAdditionalInfo = null;
        }

        if (model.BreedName == null)
        {
            ModelState.AddModelError(nameof(model.BreedName), _localizer["Select or enter the breed of your pet"]);
        }
    }

    #endregion Private Methods
}
