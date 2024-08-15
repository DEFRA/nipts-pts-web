using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetKeeperAddressManual()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.HistoryBack);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperAddressManual.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetKeeperAddressManual);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperAddressManual(PetKeeperAddressManualViewModel model, [FromHeader] string Referer)
    {
        if (Referer.Contains("PetKeeperAddress"))
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperAddress);
        }
        else
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperPostcode);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.PostcodeRegion = PostcodeRegion.GB;
        model.IsCompleted = true;
        SaveFormData(model);

        var travelDocument = TempData.GetTravelDocument();

        // clear postcode
        travelDocument.PetKeeperPostcode.ClearData();

        // clear postcode address
        travelDocument.PetKeeperAddress.ClearData();

        TempData.SetTravelDocument(travelDocument);

        return RedirectToAction(nameof(PetKeeperPhone));
    }
}
