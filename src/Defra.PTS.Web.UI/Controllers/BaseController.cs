using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace Defra.PTS.Web.UI.Controllers;

public abstract class BaseController : Controller
{
    public abstract HttpContext GetHttpContext();

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

    public override void OnActionExecuting(ActionExecutingContext context)
    {        
        if (Request.Query.ContainsKey("cy"))
        {
            HttpContext.Session.SetString("Language", "cy");
            SetCulture("cy");
        }
        else if (Request.Query.ContainsKey("en"))
        {
            HttpContext.Session.SetString("Language", "en-GB");
            SetCulture("en-GB");
        }
        else
        {            
            var savedLanguage = HttpContext.Session.GetString("Language");
            if (!string.IsNullOrEmpty(savedLanguage))
            {
                SetCulture(savedLanguage);
            }
        }

        base.OnActionExecuting(context);
    }

    private static void SetCulture(string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
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

