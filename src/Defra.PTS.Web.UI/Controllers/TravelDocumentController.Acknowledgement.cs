using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult Acknowledgement()
    {
        var applicationReference =  GetApplicationReference();
        
        if (!IsApplicationInProgress() && string.IsNullOrEmpty(applicationReference))
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        var formData = GetFormData();

        if (formData != null && !formData.DoesPageMeetPreConditions(formData.Acknowledgement.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SetBackUrl(string.Empty);

        var viewModel = formData != null ? formData.Acknowledgement : new Domain.ViewModels.TravelDocument.AcknowledgementViewModel { Reference = applicationReference};

        // Clear data
        RemoveFormData();

        return View(viewModel);
    }
}
