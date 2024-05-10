using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;

namespace Defra.PTS.Web.Application.Extensions;

public static class IPostcodeModelExtensions
{
    public static bool IsGBPostcode(this IPostcodeModel region)
    {
        return region.PostcodeRegion == PostcodeRegion.GB;
    }

    public static bool IsNonGBPostcode(this IPostcodeModel region)
    {
        return region.PostcodeRegion == PostcodeRegion.NonGB;
    }

    public static bool IsPostcodeRegionUnknown(this IPostcodeModel region)
    {
        return region.PostcodeRegion switch
        {
            PostcodeRegion.GB or PostcodeRegion.NonGB => false,
            _ => true,
        };
    }

    public static bool PostcodeStartsWithNonGBPrefix(this IPostcodeModel region)
    {
        return ApplicationHelper.PostcodeStartsWithNonGBPrefix(region.Postcode);
    }
}
