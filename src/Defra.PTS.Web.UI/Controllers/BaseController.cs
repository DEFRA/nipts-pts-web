using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Flurl.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace Defra.PTS.Web.UI.Controllers;

public abstract class BaseController : Controller
{
    public abstract HttpContext GetHttpContext();

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var cookieVal = HttpContext.Request.Cookies[".AspNetCore.Culture"];
        if (cookieVal == "\"c=cy|uic=cy\"")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cy");

        }

        base.OnActionExecuting(context);
    }

    protected void SetBackUrl(string backUrl)
    {
        ViewData.SetKeyValue(WebAppConstants.ViewKeys.BackUrl, string.IsNullOrWhiteSpace(backUrl) ? null : backUrl);
    }

    public virtual Guid CurrentUserId()
    {
        if (GetHttpContext().User.Identity.IsAuthenticated)
        {
            var userId = GetHttpContext().User.GetLoggedInUserId();
            return userId;
        }

        return Guid.Empty;
    }

    #region MagicWordFunctions

    protected void SaveMagicWordFormData(MagicWordViewModel model)
    {
        TempData.SetHasUserUsedMagicWord(model);
    }

    protected MagicWordViewModel GetMagicWordFormData(bool createIfNull = false)
    {
        return TempData.GetHasUserUsedMagicWord(createIfNull);
    }

    protected void RemoveMagicWordFormData()
    {
        TempData.RemoveHasUserUsedMagicWord();
    }

    #endregion
}

