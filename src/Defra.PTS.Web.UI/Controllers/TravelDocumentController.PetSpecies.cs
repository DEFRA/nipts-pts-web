using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{   
    [HttpGet]
    public IActionResult PetSpecies(int? id = null)
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochipDate);

        var formData = GetFormData();        
        formData.PetSpecies.PreviousSelectedSpecies = formData.PetSpecies.PetSpecies;
        SaveFormData(formData.PetSpecies);

        if (!formData.DoesPageMeetPreConditions(formData.PetSpecies.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        return View(GetFormData().PetSpecies);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetSpecies(PetSpeciesViewModel model)
    {
        bool newSpecies = false;
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochipDate);

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        model.IsCompleted = true;
        var formData = GetFormData();        

        if (formData.PetSpecies.PreviousSelectedSpecies != model.PetSpecies)
        {
            formData.PetBreed.ClearData();
            formData.PetColour.ClearData();
            SaveFormData(formData.PetBreed);
            SaveFormData(formData.PetColour);
            newSpecies = true;
        }

        SaveFormData(model);

        if (!newSpecies && model.PetSpecies == Domain.Enums.PetSpecies.Ferret)
        {
            return CYARedirect(nameof(PetBreed));
        }
   
        if (model.PetSpecies == Domain.Enums.PetSpecies.Ferret)
        {
            var redirect = GetCYACheck() ? nameof(PetColour) : nameof(PetName);
            return base.RedirectToAction(redirect);
        }

        if (!newSpecies)
        {
            return CYARedirect(nameof(PetBreed));
        }

        return RedirectToAction(nameof(PetBreed));
    }
}
