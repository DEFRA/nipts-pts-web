using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> PetKeeperAddress()
    {
        if (!IsApplicationInProgress())
        {
            return RedirectToAction(nameof(PetKeeperUserDetails));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperPostcode);

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperAddress.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        formData.PetKeeperAddress.Postcode = formData.PetKeeperPostcode.Postcode;
        if (formData.PetKeeperAddress.IsPostcodeRegionUnknown())
        {
            var validPostcode = await _mediator.Send(new ValidateGreatBritianAddressRequest(formData.PetKeeperAddress.Postcode));
            formData.PetKeeperAddress.PostcodeRegion = validPostcode ? PostcodeRegion.GB : PostcodeRegion.NonGB;
        }

        SaveFormData(formData.PetKeeperAddress);

        if (!formData.PetKeeperAddress.IsGBPostcode())
        {
            return RedirectToAction(nameof(PetKeeperPostcode));
        }

        var response = await _mediator.Send(new AddressLookupRequest(formData.PetKeeperAddress.Postcode));
        ViewBag.AddressList = response.Addresses.ToSelectListItems();

        return View(formData.PetKeeperAddress);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PetKeeperAddress(PetKeeperAddressViewModel model)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.PetKeeperPostcode);

        if (!ModelState.IsValid)
        {
            var response = await _mediator.Send(new AddressLookupRequest(model.Postcode));
            ViewBag.AddressList = response.Addresses.ToSelectListItems();

            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        // clear manual address
        var formData = GetFormData();
        formData.PetKeeperAddressManual.ClearData();
        SaveFormData(formData.PetKeeperAddressManual);

        return RedirectToAction(nameof(PetKeeperPhone));
    }
}
