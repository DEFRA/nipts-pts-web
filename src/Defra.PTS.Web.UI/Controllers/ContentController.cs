﻿using Defra.PTS.Web.UI.Constants;
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
        SetBackUrl(WebAppConstants.HistoryBack);
        
        return View();
    }

    public IActionResult AccessibilityStatement()
    {
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
    }

    public IActionResult Cookies()
    {
        SetBackUrl(WebAppConstants.HistoryBack);

        return View();
    }

    public IActionResult TermsAndConditions()
    {
        SetBackUrl(WebAppConstants.HistoryBack);
        
        return View();
    }

    [ExcludeFromCodeCoverage]
    public override HttpContext GetHttpContext()
    {
        throw new NotImplementedException();
    }
}