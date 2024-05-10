using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Extensions;

public static class ViewDataExtensions
{
    public static void SetKeyValue(this ViewDataDictionary<dynamic> viewData, string key, object value)
    {
        viewData[key] = value;
    }

    [ExcludeFromCodeCoverage]
    public static void SetKeyValue<T>(this ViewDataDictionary<T> viewData, string key, object value) where T : class
    {
        viewData[key] = value;
    }

    public static void SetKeyValue(this ViewDataDictionary viewData, string key, object value)
    {
        viewData[key] = value;
    }

    public static string GetKeyValue(this ViewDataDictionary<dynamic> viewData, string key)
    {
        string result = (viewData[key] ?? string.Empty) as string;
        return result;
    }
}
