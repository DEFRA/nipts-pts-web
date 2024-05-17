using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetKeeperPostcode()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperName);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperPostcode.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetKeeperPostcode);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperPostcode(PetKeeperPostcodeViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperName);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.PostcodeRegion = PostcodeRegion.GB;
        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetKeeperAddress));
    }
}
