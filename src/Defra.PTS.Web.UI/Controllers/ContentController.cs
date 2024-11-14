using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;

namespace Defra.PTS.Web.UI.Controllers;

[ExcludeFromCodeCoverage]
public class ContentController : BaseController
{
    private readonly IOptions<GoogleTagManager> _googleTagManager;

    public ContentController(IOptions<GoogleTagManager> googleTagManager) : base()
    {
        ArgumentNullException.ThrowIfNull(googleTagManager);
        _googleTagManager = googleTagManager;
    }

    public IActionResult Help()
    {
        //Help page
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
    }

    public IActionResult AccessibilityStatement()
    {
        //Accessibility Statement Page
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
    }

    [HttpGet]
    public IActionResult Cookies(bool saved = false)
    {
        //Cookies Page
        SetBackUrl(WebAppConstants.HistoryBack);

        //Retrieve GA cookie values from request
        var model = new CookiesModel()
        {
            GaCookieAcceptYesNo = Request.Cookies["cookie_policy"],
            MeasurementId = "_ga_" + _googleTagManager.Value.MeasurementId,
            SeenCookieMessage = Request.Cookies["seen_cookie_message"] == "yes" ? true : false,
        };


        if (!model.SeenCookieMessage)
            model.GaCookieAcceptYesNo = "reject";

        model.Saved = saved;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetCookie(CookiesModel model)
    {
        CookieOptions cookieOptions = new CookieOptions()
        {
            Expires = DateTime.UtcNow.AddMonths(12),
            IsEssential = true,
            Secure = true
        };
        Response.Cookies.Delete("seen_cookie_message");
        Response.Cookies.Delete("cookie_policy");
        Response.Cookies.Append("seen_cookie_message", "yes", cookieOptions);

        if (model.GaCookieAcceptYesNo == "reject")
        {
            Response.Cookies.Append("cookie_policy", "reject", cookieOptions);
            Response.Cookies.Delete(model.MeasurementId!, new CookieOptions { Path = "/", Domain = _googleTagManager.Value.Domain, Secure = true });
            Response.Cookies.Delete("_ga", new CookieOptions { Path = "/", Domain = _googleTagManager.Value.Domain, Secure = true });
        }
        else
        {
            Response.Cookies.Append("cookie_policy", "accept", cookieOptions);
        }

        return RedirectToAction(nameof(Cookies), new {saved = true});

    }

    public IActionResult TermsAndConditions()
    {
        //T and Cs
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
    }

    [ExcludeFromCodeCoverage]
    public override HttpContext GetHttpContext()
    {
        throw new NotImplementedException();
    }
}