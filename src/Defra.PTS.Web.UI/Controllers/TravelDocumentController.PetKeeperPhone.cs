using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetKeeperPhone()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperAddress);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperPhone.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetKeeperPhone);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperPhone(PetKeeperPhoneViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperPostcode);

        if (!ModelState.IsValid)
        {   
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetMicrochip));
    }
}
