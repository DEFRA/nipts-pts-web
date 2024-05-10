using Microsoft.AspNetCore.Mvc.Razor;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Extensions;

[ExcludeFromCodeCoverage]
public static class RazorPageExtensions
{
    public static void SetMetaTitle(this RazorPageBase page, string metaTitle)
    {
        page.ViewBag.Title = metaTitle;
    }
}
