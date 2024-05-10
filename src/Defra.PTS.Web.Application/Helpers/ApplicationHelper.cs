using Defra.PTS.Web.Application.Constants;

namespace Defra.PTS.Web.Application.Helpers;

public static class ApplicationHelper
{
    public static string MapStatusToDisplayStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return string.Empty;
        }

        return status.Trim().ToUpperInvariant() switch
        {
            "AUTHORIZED" or "AUTHORISED" or "APPROVED" => AppConstants.ApplicationStatus.APPROVED,
            "AWAITING VERIFICATION" => AppConstants.ApplicationStatus.AWAITINGVERIFICATION,
            "REJECTED" or "UNSUCCESSFUL" => AppConstants.ApplicationStatus.UNSUCCESSFUL,
            "REVOKED" => AppConstants.ApplicationStatus.REVOKED,
            _ => status,
        };
    }

    public static bool PostcodeStartsWithNonGBPrefix(string postcode)
    {
        if (string.IsNullOrEmpty(postcode))
        {
            return false;
        }

        // Non GB : Ireland postcodes will start with BT, JE, GY and IM
        var nonGBPrefixes = new List<string>
        {
            "BT",
            "JE",
            "GY",
            "IM"
        };

        return nonGBPrefixes.Exists(x => postcode.ToUpper().StartsWith(x));
    }
}
