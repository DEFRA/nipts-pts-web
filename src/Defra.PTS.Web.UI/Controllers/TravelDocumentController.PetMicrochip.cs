using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetMicrochip()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(PetMicrochippageBackUrl());

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetMicrochip.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetMicrochip);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetMicrochip(PetMicrochipViewModel model)
    {
        SetBackUrl(PetMicrochippageBackUrl());

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        if (model.Microchipped == YesNoOptions.No)
        {
            return RedirectToAction(nameof(PetMicrochipNotAvailable));
        }

        return CYARedirect(nameof(PetMicrochipDate));
    }

    [HttpGet]
    public IActionResult PetMicrochipNotAvailable()
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochip);

        return View(GetFormData().PetMicrochipNotAvailable);
    }

    private string PetMicrochippageBackUrl()
    {
        var formData = GetFormData();

        if (formData.PetKeeperUserDetails.UserDetailsAreCorrect == YesNoOptions.Yes)
        {
            return WebAppConstants.Pages.TravelDocument.PetKeeperUserDetails;
        }

        return WebAppConstants.Pages.TravelDocument.PetKeeperPhone;
    }
}
