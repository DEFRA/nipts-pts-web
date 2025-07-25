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
            AppConstants.ApplicationStatus.REVOKED => "govuk-tag--red",
            AppConstants.ApplicationStatus.UNSUCCESSFUL => "govuk-tag--orange",
            AppConstants.ApplicationStatus.APPROVED => "govuk-tag--green",
            AppConstants.ApplicationStatus.AWAITINGVERIFICATION => "govuk-tag--blue",
            AppConstants.ApplicationStatus.SUSPENDED => "govuk-tag--yellow",
            _ => "govuk-tag--red",
        };

        return cssClass;
    }

    public static string StatusBasedDetailsUrl(this ApplicationSummaryDto application)// to change, to include cancelled and unsuccessful
    {
        string cssClass = application.Status switch
        {
            AppConstants.ApplicationStatus.APPROVED => $"{WebAppConstants.Pages.TravelDocument.ApplicationCertificate}/{application.ApplicationId}",
            AppConstants.ApplicationStatus.SUSPENDED => $"{WebAppConstants.Pages.TravelDocument.ApplicationCertificate}/{application.ApplicationId}",
            AppConstants.ApplicationStatus.REVOKED => $"{WebAppConstants.Pages.TravelDocument.ApplicationCertificate}/{application.ApplicationId}",
            AppConstants.ApplicationStatus.UNSUCCESSFUL => $"{WebAppConstants.Pages.TravelDocument.ApplicationDetails}/{application.ApplicationId}",
            _ => $"{WebAppConstants.Pages.TravelDocument.ApplicationDetails}/{application.ApplicationId}",
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
            if (Thread.CurrentThread.CurrentCulture.EnglishName == "Welsh")
            {
                result.Insert(0, new SelectListItem { Text = $"{result.Count} o gyfeiriadau wedi'u canfod", Value = string.Empty });
            }
            else
            {
                result.Insert(0, new SelectListItem { Text = $"{result.Count} addresses found", Value = string.Empty });
            }
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
            result.Insert(0, new SelectListItem { Text = string.Empty, Value = "0" });
        }

        return result;
    }
}
