
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Helpers;
public interface IHostHelper
{
    /// <summary>
    /// Will return the external facing URL of the app service based on if the app is running in staging or not
    /// </summary>
    /// <returns></returns>
    string HostUrl();

    /// <summary>
    /// Returns True if the app is running on a staging app service and false if not
    /// </summary>
    /// <returns></returns>
    bool IsStaging();
}

[ExcludeFromCodeCoverage]
public class HostHelper : IHostHelper
{
    private static readonly string[] CheckHeaders = new[] { "x-original-host", "host" };
    private readonly IConfiguration configuration;
    private readonly IHttpContextAccessor contextAccessor;

    public HostHelper(IConfiguration configuration, IHttpContextAccessor contextAccessor)
    {
        this.configuration = configuration;
        this.contextAccessor = contextAccessor;
    }

    public string HostUrl()
    {
        var staging = configuration.GetSection("AppOptions:AppUrls:Staging").Value;
        var live = configuration.GetSection("AppOptions:AppUrls:Live").Value;

        return IsStaging() ? staging : live;
    }

    public bool IsStaging()
    {
        var context = contextAccessor.HttpContext;
        return context == null
            ? IsStagingFromEnv()
            : IsStagingFromHeaders(context);
    }

    private static bool IsStagingFromHeaders(HttpContext context)
    {
        foreach (var key in CheckHeaders)
        {
            if (context.Request.Headers.TryGetValue(key, out var header))
            {
                return header.ToString().Contains("-staging");
            }
        }

        return IsStagingFromEnv();
    }

    private static bool IsStagingFromEnv()
    {
        var hostSiteName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
        return hostSiteName != null && hostSiteName.Contains("-staging");
    }
}

