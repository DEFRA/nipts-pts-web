using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUglify.JavaScript.Syntax;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Controllers;

[Authorize]
public class BaseTravelDocumentController : BaseController
{
    #region Application

    public void EnsureApplicationPagePreConditions(TravelDocumentFormPageType pageType)
    {
        if (!IsApplicationInProgress())
        {
            RedirectToAction(nameof(Index)).ExecuteResult(this.ControllerContext);
        }

        var formData = GetFormData();
        if (!formData.DoesPageMeetPreConditions(pageType, out string actionName))
        {
            RedirectToAction(actionName).ExecuteResult(this.ControllerContext);
        }
    }

    public virtual bool IsApplicationInProgress()
    {
        var formData = GetFormData();

        var magicWordData = GetMagicWordFormData();

        if (magicWordData != null && !magicWordData.HasUserPassedPasswordCheck)
            return false;

        if (formData != null && formData.IsApplicationInProgress)
            return true;
        else
            return false;
    }

    protected void SetApplicationIsSubmitted(bool isSubmitted = true)
    {
        var formData = GetFormData();

        formData.IsSubmitted = isSubmitted;

        SaveFormData(formData);
    }

    protected void SetApplicationInProgress(bool isApplicationInProgress = true)
    {
        var formData = GetFormData();

        formData.IsApplicationInProgress = isApplicationInProgress;

        SaveFormData(formData);
    }

    protected void ExcludePetKeeperOtherDetails()
    {
        var formData = GetFormData();

        formData.PetKeeperName.IsCompleted = false;
        formData.PetKeeperPostcode.IsCompleted = false;
        formData.PetKeeperAddress.IsCompleted = false;
        formData.PetKeeperAddressManual.IsCompleted = false;
        formData.PetKeeperPhone.IsCompleted = false;

        SaveFormData(formData);
    }
    #endregion Application

    #region SaveFormData

    protected void SaveFormData(AcknowledgementViewModel model)
    {
        var formData = GetFormData();
        
        formData.Acknowledgement = model;        

        SaveFormData(formData);
        SaveApplicationReference(model.Reference);
    }

    protected void SaveFormData(DeclarationViewModel model)
    {
        var formData = GetFormData();

        formData.Declaration = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetAgeViewModel model)
    {
        var formData = GetFormData();

        formData.PetAge = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetBreedViewModel model)
    {
        var formData = GetFormData();

        formData.PetBreed = model;

        SaveFormData(formData);
    }

    public virtual void SaveFormData(PetColourViewModel model)
    {
        var formData = GetFormData();

        formData.PetColour = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetFeatureViewModel model)
    {
        var formData = GetFormData();

        formData.PetFeature = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetGenderViewModel model)
    {
        var formData = GetFormData();

        formData.PetGender = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperAddressManualViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperAddressManual = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperAddressViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperAddress = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperNameViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperName = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperPhoneViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperPhone = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperPostcodeViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperPostcode = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetKeeperUserDetailsViewModel model)
    {
        var formData = GetFormData();

        formData.PetKeeperUserDetails = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetMicrochipDateViewModel model)
    {
        var formData = GetFormData();

        formData.PetMicrochipDate = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetMicrochipViewModel model)
    {
        var formData = GetFormData();

        formData.PetMicrochip = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetNameViewModel model)
    {
        var formData = GetFormData();

        formData.PetName = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(PetSpeciesViewModel model)
    {
        var formData = GetFormData();

        formData.PetSpecies = model;

        SaveFormData(formData);
    }

    protected void SaveFormData(TravelDocumentViewModel model)
    {
        TempData.SetTravelDocument(model);
    }
    #endregion SaveFormData

    #region TravelDocument
    public virtual TravelDocumentViewModel GetFormData(bool createIfNull = false)
    {
        return TempData.GetTravelDocument(createIfNull);
    }

    protected void RemoveFormData()
    {
        TempData.RemoveTravelDocument();
    }
    #endregion TravelDocument

    #region FormSubmissionQueue
    public void AddToFormSubmissionQueue(Guid id)
    {
        TempData.AddToFormSubmissionQueue(id);
    }
    public void RemoveFromFormSubmissionQueue(Guid id)
    {
        TempData.RemoveFromFormSubmissionQueue(id);
    }
    
    public bool IsInFormSubmissionQueue(Guid id)
    {
        return TempData.IsInFormSubmissionQueue(id);
    }

    [ExcludeFromCodeCoverage]
    public override HttpContext GetHttpContext()
    {
        throw new NotImplementedException();
    }
    #endregion FormSubmissionQueue

    #region ApplicationReference
    protected void SaveApplicationReference(string applicationReference)
    {
        TempData.SetApplicationReference(applicationReference);
    }
    public string GetApplicationReference()
    {
        return TempData.GetApplicationReference();
    }
    #endregion ApplicationReference
    public void SetCYACheck()
    {
        TempData.Set("CYA", "Yes");
    }

    public virtual bool GetCYACheck()
    {
        var CYA = TempData.Peek("CYA");
        if (CYA != null && CYA.ToString().Contains("Yes"))
        {
            return true;
        }
        return false;
    }

    public IActionResult CYARedirect(string actionResult)
    {
        if (GetCYACheck())
        {
            return RedirectToAction(nameof(Declaration));
        }
        return RedirectToAction(actionResult);
    }
}
