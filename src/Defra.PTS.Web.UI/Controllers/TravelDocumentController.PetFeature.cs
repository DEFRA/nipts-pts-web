using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetFeature()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetColour);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetFeature.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetFeature);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetFeature(PetFeatureViewModel model)
    {
        model.TrimUnwantedData();
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetColour);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(Declaration));
    }
}
