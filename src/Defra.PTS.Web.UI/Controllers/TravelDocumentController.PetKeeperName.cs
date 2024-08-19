using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetKeeperName()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackLink();

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperName.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetKeeperName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperName(PetKeeperNameViewModel model)
    {
        SetBackLink();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetKeeperPostcode));
    }

    private void SetBackLink()
    {
        var formData = GetFormData();
        if (formData.PetKeeperUserDetails.PostcodeRegion == PostcodeRegion.GB)
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperUserDetails);
        }
        else
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperNonGbAddress);
        }
    }
}
