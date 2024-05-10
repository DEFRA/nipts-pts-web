using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetName()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetName.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        if (formData.PetSpecies.PetSpecies == Domain.Enums.PetSpecies.Ferret)
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetSpecies);
        }
        else
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetBreed);
        }

        return View(formData.PetName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetName(PetNameViewModel model)
    {
        var formData = GetFormData();
        if (formData.PetSpecies.PetSpecies == Domain.Enums.PetSpecies.Ferret)
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetSpecies);
        }
        else
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetBreed);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetGender));
    }
}
