using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.TravelDocument.Commands;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public IActionResult Declaration()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(Index));
        }

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.Declaration.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SetCYACheck("Yes");
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetFeature);

        var address = formData.GetPetOwnerAddress();

        formData.Declaration.Postcode = address.PostCode;
        formData.Declaration.IsManualAddress = formData.PetKeeperAddressManual.IsCompleted;
        formData.Declaration.Phone = formData.GetPetOwnerPhone();
        formData.Declaration.RequestId = formData.RequestId;

        SaveFormData(formData.Declaration);

        return View(formData.Declaration);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Declaration(DeclarationViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetFeature);

        if (IsInFormSubmissionQueue(model.RequestId) || !ModelState.IsValid)
        {
            return View(model);
        }

        var formData = GetFormData();
        if (formData.IsSubmitted)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        if (IsInFormSubmissionQueue(formData.RequestId))
        {
            return View(model);
        }
        else
        {
            AddToFormSubmissionQueue(formData.RequestId);
        }

        try
        {
            var userInfo = GetCurrentUserInfo();
            var response = await _mediator.Send(new CreateTravelDocumentRequest(formData, userInfo));

            var acknowledgementModel = new AcknowledgementViewModel
            {
                IsCompleted = true,
                IsSuccess = response.IsSuccess,
                Reference = response.Reference
            };

            SaveFormData(acknowledgementModel);
            SetApplicationIsSubmitted(isSubmitted: true);
            SetCYACheck("No");
            return RedirectToAction(nameof(Acknowledgement));
        }
        catch (Exception ex)
        {
            SetApplicationIsSubmitted(isSubmitted: false);
            RemoveFromFormSubmissionQueue(formData.RequestId);
            _logger.LogError(ex, "Exception Message: {0}", ex.Message);
            throw;
        }

    }
}
