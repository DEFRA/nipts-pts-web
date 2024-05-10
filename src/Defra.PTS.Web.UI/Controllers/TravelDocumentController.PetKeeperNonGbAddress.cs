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
    public async Task<IActionResult> PetKeeperNonGbAddressAsync()
    {
        var magicWordData = GetMagicWordFormData();
        if (magicWordData != null && !magicWordData.HasUserPassedPasswordCheck)
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        var formData = GetFormData(createIfNull: true);
        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperUserDetails.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SaveFormData(formData);

        var userDetail = await InitializeUserDetails();
        if (userDetail != null)
        {
            formData.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
            formData.PetKeeperUserDetails.Name = userDetail.FullName;
            formData.PetKeeperUserDetails.Email = userDetail.Email;
            formData.PetKeeperUserDetails.Phone = userDetail.Telephone;
            formData.PetKeeperUserDetails.AddressLineOne = userDetail.AddressLineOne + " " + userDetail.AddressLineTwo;
            formData.PetKeeperUserDetails.TownOrCity = userDetail.TownOrCity;
            formData.PetKeeperUserDetails.Postcode = userDetail.PostCode;

            SaveFormData(formData.PetKeeperUserDetails);
        }

        if (formData.PetKeeperUserDetails.IsPostcodeRegionUnknown())
        {
            var validGBAddress = await _mediator.Send(new ValidateGreatBritianAddressRequest(userDetail?.PostCode));

            formData.PetKeeperUserDetails.PostcodeRegion = validGBAddress ? PostcodeRegion.GB : PostcodeRegion.NonGB;
            SaveFormData(formData.PetKeeperUserDetails);
        }

        return View(formData.PetKeeperUserDetails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperNonGbAddress(PetKeeperUserDetailsViewModel model)
    {        
        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        model.IsCompleted = true;
        SaveFormData(model);

        SetApplicationInProgress();

        return RedirectToAction(nameof(PetKeeperName));
    }


}
