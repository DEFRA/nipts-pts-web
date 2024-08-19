using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetGender()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetName);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetGender.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetGender);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetGender(PetGenderViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetName);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return RedirectToAction(nameof(PetAge));
    }
}
