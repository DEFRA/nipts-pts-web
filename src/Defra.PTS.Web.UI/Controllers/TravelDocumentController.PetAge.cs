using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetAge()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetGender);

        var formData = GetFormData();
        formData.PetAge.MicrochippedDate = formData.PetMicrochipDate.MicrochippedDate.GetValueOrDefault();
        SaveFormData(formData.PetAge);

        if (!formData.DoesPageMeetPreConditions(formData.PetAge.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(formData.PetAge);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetAge(PetAgeViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetGender);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return CYARedirect(nameof(PetColour));
    }
}
