using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Defra.PTS.Web.UI.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpRequestExtensions
{
    public static bool IsLocal(this HttpRequest req)
    {
        var connection = req.HttpContext.Connection;
        if (connection.RemoteIpAddress != null)
        {
            if (connection.LocalIpAddress != null)
            {
                return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
            }
            else
            {
                return IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
        }

        // for in memory TestServer or when dealing with default connection info  
        if (connection.RemoteIpAddress == null && connection.LocalIpAddress == null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns host url e.g. http://localhost:5000
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static string GetHostUrl(this HttpRequest req)
    {
        if (!req.Host.HasValue)
        {
            return string.Empty;
        }

        var httpPrefix = req.IsHttps ? "https" : "http";
        return $"{httpPrefix}://{req.Host.Value}";
    }
}
