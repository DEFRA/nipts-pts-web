using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Controllers;

[ExcludeFromCodeCoverage]
public class ErrorController : BaseController
{

    public ErrorController() : base()
    {
    }

    public override HttpContext GetHttpContext()
    {
        throw new NotImplementedException();
    }

    [Route("/Error/HandleError/{code:int}")]
    public IActionResult HandleError(int code, string backLink = "")
    {
        SetBackUrl(backLink);

        ViewData.SetKeyValue("ErrorMessage", $"Error occurred. The ErrorCode is: {code}");
        switch (code)
        {
            case 401:
                return View(WebAppConstants.Views.AccessDenied);
            case 404:
                return View(WebAppConstants.Views.PageNotFound);
            case 403:
                return View(WebAppConstants.Views.Forbidden);
            case 500:
                return View(WebAppConstants.Views.InternalServer);
            default:
                return View(WebAppConstants.Views.HandleError);
        }
    }
}
