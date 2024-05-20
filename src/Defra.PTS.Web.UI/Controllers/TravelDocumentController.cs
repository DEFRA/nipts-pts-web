using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Features.Users.Commands;
using Defra.PTS.Web.Application.Features.Users.Queries;

using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Defra.PTS.Web.UI.Controllers;

[Authorize]
public partial class TravelDocumentController : BaseTravelDocumentController
{

    private readonly IValidationService _validationService;
    private readonly IMediator _mediator;
    private readonly ILogger<TravelDocumentController> _logger;
    private readonly PtsSettings _ptsSettings;


    public TravelDocumentController(
          IValidationService validationService,
          IMediator mediator,
          ILogger<TravelDocumentController> logger,
          IOptions<PtsSettings> ptsSettings
          )
    {
        ArgumentNullException.ThrowIfNull(validationService);
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(ptsSettings);

        _validationService = validationService;
        _mediator = mediator;
        _logger = logger;
        _ptsSettings = ptsSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (Response != null && Response.Headers != null)
            {
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                Response.Headers.Add("Pragma", "no-cache");
                Response.Headers.Add("Expires", "0");
            }

            var magicWordData = GetMagicWordFormData(true);

            if (_ptsSettings.MagicWordEnabled && magicWordData != null && !magicWordData.HasUserPassedPasswordCheck)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                SaveMagicWordFormData(new MagicWordViewModel { HasUserPassedPasswordCheck = true });

                SetBackUrl(string.Empty);

                await AddOrUpdateUser();
                await InitializeUserDetails();

                var statuses = new List<string>()
                {
                    AppConstants.ApplicationStatus.APPROVED,
                    AppConstants.ApplicationStatus.AWAITINGVERIFICATION,
                };

                var userId = CurrentUserId();
                var response = await _mediator.Send(new GetApplicationsQueryRequest(userId, statuses));
                return View(response.Applications);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error has occurred");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ApplicationDetailRecord(string id, string status)
    {
        
        HttpContext.Session.SetString("ApplicationId", id);        

        if (status == AppConstants.ApplicationStatus.APPROVED)
            return RedirectToAction(nameof(ApplicationCertificate));
        else
            return RedirectToAction(nameof(ApplicationDetails));


    }

    #region Private Methods
    [ExcludeFromCodeCoverage]
    private async Task AddOrUpdateUser()
    {
        try
        {
            var userInfo = GetCurrentUserInfo();

            var response = await _mediator.Send(new AddUserRequest(userInfo));
            if (response.IsSuccess)
            {
                var identity = HttpContext.User.Identities.FirstOrDefault();
                if (identity != null)
                {
                    identity.AddClaim(new Claim(WebAppConstants.IdentityKeys.PTSUserId, response.UserId.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to save user details", ex);
            throw;
        }
    }

    [ExcludeFromCodeCoverage]
    private async Task<UserDetailDto> InitializeUserDetails()
    {
        // Save user
        var userInfo = GetCurrentUserInfo();

        try
        {
            await _mediator.Send(new AddAddressRequest(userInfo));
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to save address details", ex);
        }
        var contactId = CurrentUserContactId();
        var response = await _mediator.Send(new GetUserDetailQueryRequest(contactId));
        if (response != null && response.UserDetail != null)
        {

            HttpContext.Session.SetString("FullName", response.UserDetail?.FullName);
            return response.UserDetail;
        }

        return null;
    }


    public override HttpContext GetHttpContext()
    {
        return HttpContext;
    }

    public Guid CurrentUserContactId()
    {
        if (GetHttpContext().User.Identity.IsAuthenticated)
        {
            var contactId = GetHttpContext().User.GetLoggedInContactId();
            return contactId;
        }

        return Guid.Empty;
    }

    public User GetCurrentUserInfo()
    {
        var identity = GetHttpContext().User.Identities.FirstOrDefault();
        if (identity == null)
        {
            return new User();
        }

        var claims = identity.Claims;

        var user = new User();
        foreach (var claim in claims)
        {
            switch (claim.Type)
            {
                case "contactId":
                    user.ContactId = claim.Value;
                    break;
                case "uniqueReference":
                    user.UniqueReference = claim.Value;
                    break;
                case "firstName":
                    user.FirstName = claim.Value;
                    break;
                case "lastName":
                    user.LastName = claim.Value;
                    break;
                case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                    user.EmailAddress = claim.Value;
                    break;
                case "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
                    user.Role = claim.Value;
                    break;
                default:
                    break;
            }
        }

        return user;
    }

    #endregion Private Methods
}
