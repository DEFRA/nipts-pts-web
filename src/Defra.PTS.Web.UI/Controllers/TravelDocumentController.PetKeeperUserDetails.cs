﻿using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.UI.Controllers;

public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> PetKeeperUserDetails()
    {
        var magicWordData = GetMagicWordFormData();
        if (_ptsSettings.MagicWordEnabled && (magicWordData == null || !magicWordData.HasUserPassedPasswordCheck))
        {
            return RedirectToAction(nameof(Index));
        }

        var formData = GetFormData(createIfNull: true);

        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);        

        if (!formData.DoesPageMeetPreConditions(formData.PetKeeperUserDetails.PageType, out string actionName))
        {
            return RedirectToAction(actionName);
        }

        SaveFormData(formData);

        var userDetail = await InitializeUserDetails();
        if (userDetail != null)
        {
            formData.PetKeeperUserDetails.Name = userDetail.FullName;
            formData.PetKeeperUserDetails.Email = userDetail.Email;
            formData.PetKeeperUserDetails.Phone = userDetail.Telephone;
            formData.PetKeeperUserDetails.AddressLineOne = userDetail.AddressLineOne + " " + userDetail.AddressLineTwo;
            formData.PetKeeperUserDetails.TownOrCity = userDetail.TownOrCity;
            formData.PetKeeperUserDetails.County = userDetail.County;
            formData.PetKeeperUserDetails.Postcode = userDetail.PostCode;

            SaveFormData(formData.PetKeeperUserDetails);
        }

        var nonGbPos = ApplicationHelper.PostcodeStartsWithNonGBPrefix(formData.PetKeeperUserDetails.Postcode);

        formData.PetKeeperUserDetails.PostcodeRegion = !nonGbPos ? PostcodeRegion.GB : PostcodeRegion.NonGB;
        SaveFormData(formData.PetKeeperUserDetails);

        if (!formData.PetKeeperUserDetails.IsGBPostcode())
        {
            return RedirectToAction(nameof(PetKeeperNonGbAddress));
        }

        return View(formData.PetKeeperUserDetails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PetKeeperUserDetails(PetKeeperUserDetailsViewModel model)
    {
        var magicWordData = GetMagicWordFormData();
        if (magicWordData != null && !magicWordData.HasUserPassedPasswordCheck)
        {
            return RedirectToAction(nameof(Index));
        }

        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.IsCompleted = true;
        SaveFormData(model);

        SetApplicationInProgress();

        if (model.UserDetailsAreCorrect == YesNoOptions.Yes)
        {
            var formData = GetFormData();

            formData.PetKeeperName = new PetKeeperNameViewModel
            {
                Name = model.Name
            };
            formData.PetKeeperPhone = new PetKeeperPhoneViewModel
            {
                Phone = model.Phone
            };

            formData.PetKeeperPostcode = new PetKeeperPostcodeViewModel { 
                Postcode = model.Postcode,
                PostcodeRegion = PostcodeRegion.GB
            };
            formData.PetKeeperAddress = new PetKeeperAddressViewModel
            {
                Postcode = model.Postcode
            };
            formData.PetKeeperAddressManual = new PetKeeperAddressManualViewModel {
                AddressLineOne = model.AddressLineOne,
                TownOrCity = model.TownOrCity,
                County = model.County,
                Postcode = model.Postcode,
                PostcodeRegion = PostcodeRegion.GB
            };

            SaveFormData(formData);

            return RedirectToAction(nameof(PetMicrochip));
        }

        return RedirectToAction(nameof(PetKeeperName));
    }
        
}
