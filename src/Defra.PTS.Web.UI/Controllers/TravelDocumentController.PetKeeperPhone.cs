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
            return RedirectToAction(nameof(Index));
        }                

        var formData = GetFormData();

        SetBackUrlManualOrLookupAddress(formData);

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
        SetBackUrlManualOrLookupAddress(GetFormData());

        if (!ModelState.IsValid)
        {   
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetMicrochip));
    }

    public void SetBackUrlManualOrLookupAddress(TravelDocumentViewModel model)
    {
        if (model.PetKeeperAddress.IsCompleted)
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperAddress);
        }
        else
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperAddressManual);
        }
    }
}
