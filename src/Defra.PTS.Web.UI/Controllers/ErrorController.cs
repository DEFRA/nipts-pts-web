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


    [Route("/Error/HandleError/{code:int}")]
    public IActionResult HandleError(int code, string backLink = "")
    {
        SetBackUrl(backLink);

        ViewData.SetKeyValue("ErrorMessage", $"Error occurred. The ErrorCode is: {code}");
        if (code == 401)
        {
            return View(WebAppConstants.Views.AccessDenied);
        }

        if (code == 404)
        {
            return View(WebAppConstants.Views.PageNotFound);
        }

        return View(WebAppConstants.Views.HandleError);
    }
}
