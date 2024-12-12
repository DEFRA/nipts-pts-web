using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult PetMicrochipDate(int? id = null)
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochip);

        // Microchip Date can't be before BirthDate
        var formData = GetFormData();

        if (!formData.DoesPageMeetPreConditions(formData.PetMicrochipDate.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        if (formData.PetAge != null && formData.PetAge.IsCompleted)
        {
            formData.PetMicrochipDate.BirthDate = formData.PetAge.BirthDate;    
        }
        else
        {
            formData.PetMicrochipDate.BirthDate = null;
        }

        SaveFormData(formData.PetMicrochipDate);

        return View(formData.PetMicrochipDate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetMicrochipDate(PetMicrochipDateViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochip);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        return CYARedirect(nameof(PetSpecies));
    }
}
