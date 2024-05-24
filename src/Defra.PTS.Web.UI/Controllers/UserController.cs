using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.Users.Commands;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.UI.Configuration.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;
using static Defra.PTS.Web.UI.Constants.WebAppConstants;

namespace Defra.PTS.Web.UI.Controllers;

[Authorize]
[ExcludeFromCodeCoverage]
public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;

    public UserController(IMediator mediator, ILogger<UserController> logger, IConfiguration configuration) : base()
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(configuration);

        _mediator = mediator;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult ManageAccount()
    {
        SetBackUrl(string.Empty);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public async Task<IActionResult> OSignOutAsync()
    {
        // Audit log user logout
        try
        {
            var userInfo = GetCurrentUserInfo();
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.EmailAddress))
            {
                var request = new UpdateUserRequest(userInfo.EmailAddress);
                _ = await _mediator.Send(request);
                MemoryCache.Default[Pages.User.AccessTokenKey] = null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateUserRequest)}: Unable to update user", ex);
            // do not throw the exception as its optional audit log call
        }

        var adB2cSection = _configuration.GetSection("AzureAdB2C").Get<OpenIdConnectB2CConfiguration>();

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync("Cookies");
        HttpContext.User = null;

        TempData.RemoveTravelDocument();
        return Redirect(adB2cSection.SignedOutCallbackPath);
    }


    [HttpGet]
    public IActionResult CheckIdm2SignOut()
    {        
        try
        {            
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();
            HttpContext.Response.Cookies.Append("ManagementLinkClicked", "false");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error", ex);
            
        }
        return RedirectToAction("Index", "TravelDocument");
    }

    [HttpGet]
    public IActionResult RedirectToExternal()
    {
        HttpContext.Response.Cookies.Append("ManagementLinkClicked", "true");
        string managementUrl = _configuration["AppSettings:ManagementUrl"];
        return Redirect(managementUrl);
    }

    [HttpGet]
    public IActionResult TempToken()
    {
        var useTempToken = _configuration.GetValue<bool>("AppSettings:UseTempToken");
        if (useTempToken)
        {
            string authToken = MemoryCache.Default[Pages.User.AccessTokenKey] != null ? MemoryCache.Default[Pages.User.AccessTokenKey].ToString() ?? string.Empty : string.Empty;
            ViewBag.Token = authToken;
        }
        else
        {
            ViewBag.Token = "Please sign out & singin again to view/copy the token";
        }

        return View();
    }


    public override HttpContext GetHttpContext()
    {
        return HttpContext;
    }

    protected User GetCurrentUserInfo()
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


}
