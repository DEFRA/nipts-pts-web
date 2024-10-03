using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Controllers;

[ExcludeFromCodeCoverage]
public class ContentController : BaseController
{
    public ContentController() : base()
    {
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

    public IActionResult Cookies()
    {
        //Cookies Page
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
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