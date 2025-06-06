﻿using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.Enums;

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

        return nonGBPrefixes.Exists(x => postcode.StartsWith(x, StringComparison.CurrentCultureIgnoreCase));
    }

    public static string BuildPdfDownloadFilename(string referenceNumber)
    {
        return $"{referenceNumber}.pdf";
    }

    public static string BuildQRCodeUrl(string base64String)
    {
        return $"data:image/png;base64,{base64String}";
    }

    public static string BuildPdfDownloadUrl(Guid id, string referenceNumber, PdfType pdfType)
    {
        var pdfAction = pdfType == PdfType.Certificate ? "DownloadCertificatePdf" : "DownloadApplicationDetailsPdf";
        return $"/TravelDocument/{pdfAction}/{id}/{referenceNumber}";
    }

    public static string FormatPetTravelDocumentNumber(string documentNumber)
    {
        if (string.IsNullOrEmpty(documentNumber) || documentNumber.Length < 11)
        {
            return documentNumber;
        }

        return $"{documentNumber[..5]} {documentNumber.Substring(5, 3)} {documentNumber[8..]}";
    }
}
