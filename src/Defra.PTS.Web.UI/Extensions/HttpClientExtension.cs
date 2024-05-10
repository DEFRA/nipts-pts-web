using System.Net.Http.Headers;
using System.Runtime.Caching;

namespace Defra.PTS.Web.UI.Extensions;

public static class HttpClientExtension
{
    public static void AddHeaderAccessToken(this HttpClient httpClient)
    {
        var accessTokenValue = MemoryCache.Default["accessToken"] ?? string.Empty;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenValue.ToString());
    }
}
