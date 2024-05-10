using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult Acknowledgement()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.Acknowledgement.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SetBackUrl(string.Empty);

        var viewModel = formData.Acknowledgement;

        // Clear data
        RemoveFormData();

        return View(viewModel);
    }
}
