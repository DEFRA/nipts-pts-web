using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Defra.PTS.Web.Application.Extensions;

public static class MiscExtensions
{
    public static string StatusBasedCssClass(this ApplicationSummaryDto application)
    {
        string cssClass = application.Status switch
        {
            AppConstants.ApplicationStatus.UNSUCCESSFUL or AppConstants.ApplicationStatus.REVOKED => "govuk-tag--red",
            AppConstants.ApplicationStatus.APPROVED => "govuk-tag--green",
            AppConstants.ApplicationStatus.AWAITINGVERIFICATION => "govuk-tag--yellow",
            _ => "govuk-tag--red",
        };

        return cssClass;
    }

    public static string StatusBasedDetailsUrl(this ApplicationSummaryDto application)
    {
        string cssClass = application.Status switch
        {
            AppConstants.ApplicationStatus.APPROVED => WebAppConstants.Pages.TravelDocument.ApplicationCertificate,
            _ => WebAppConstants.Pages.TravelDocument.ApplicationDetails,
        };

        return cssClass;
    }

    public static List<SelectListItem> ToSelectListItems(this List<Address> addressList, bool hasSelectRow = true)
    {
        var result = addressList
            .Select(x => new SelectListItem
            {
                Text = x.ToDisplayString(),
                Value = x.ToCsvString()
            })
            .ToList();

        // Select option
        if (hasSelectRow)
        {
            result.Insert(0, new SelectListItem { Text = $"{result.Count} addresses found", Value = string.Empty });
        }

        return result;
    }

    public static List<SelectListItem> ToSelectListItems(this List<BreedDto> breeds, bool hasSelectRow = true)
    {
        var result = breeds
            .Select(x => new SelectListItem
            {
                Text = x.BreedName,
                Value = x.BreedId.ToString()
            })
            .ToList();

        if (hasSelectRow)
        {
            result.Add(new SelectListItem { Text = string.Empty, Value = "0" });
        }

        return result;
    }
}
