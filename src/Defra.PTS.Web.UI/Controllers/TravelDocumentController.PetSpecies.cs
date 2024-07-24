using Defra.PTS.Web.Domain.Enums;
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
            return RedirectToAction(nameof(PetKeeperUserDetails));
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
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetMicrochipDate);

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        model.IsCompleted = true;
        var formData = GetFormData();        

        if (formData.PetSpecies.PreviousSelectedSpecies != model.PetSpecies)
        {            
            formData.PetBreed.BreedId = 0;
            formData.PetBreed.BreedName = "";
            SaveFormData(formData.PetBreed);
        }

        SaveFormData(model);
   
        if (model.PetSpecies == Domain.Enums.PetSpecies.Ferret)
        {
            return base.RedirectToAction(nameof(PetName));
        }

        return RedirectToAction(nameof(PetBreed));
    }
}
