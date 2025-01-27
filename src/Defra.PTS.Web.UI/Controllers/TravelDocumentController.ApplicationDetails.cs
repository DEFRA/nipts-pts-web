﻿using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Controllers;

[Authorize]
public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet("/TravelDocument/ApplicationDetails/{id}")]
    public async Task<IActionResult> ApplicationDetails(Guid id)
    {
        try
        {
            SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

            var response = await _mediator.Send(new GetApplicationDetailsQueryRequest(id));

            var userId = CurrentUserId();
            if (!response.ApplicationDetails.UserId.Equals(userId))
            {
                return RedirectToAction("HandleError", "Error", new { code = 404 });
            }

            return View(response.ApplicationDetails);
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            // Log the error if necessary
            // Redirect to the error handler with the specific error code
            return RedirectToAction("HandleError", "Error", new { code = (int)ex.StatusCode.Value });
        }
        catch (Exception)
        {
            // Log the error
            // Redirect to a generic server error handler (500)
            return RedirectToAction("HandleError", "Error", new { code = 500 });
        }
    }

    [ExcludeFromCodeCoverage]
    [HttpGet("/TravelDocument/DownloadApplicationDetailsPdf/{id}/{referenceNumber}")]
    public async Task<IActionResult> DownloadApplicationDetailsPdf(Guid id, string referenceNumber)
    {
        try
        {
            var userId = CurrentUserId();

            var response = await _mediator.Send(new GenerateApplicationPdfRequest(id, userId));
            if (response == null)
            {
                return new NotFoundObjectResult("Unable to download the PDF");
            }

            var fileName = ApplicationHelper.BuildPdfDownloadFilename(referenceNumber);
            var fileTitle = "Application number: " + referenceNumber + ".pdf";

            return await SetFileTitle(response, fileName, fileTitle);
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            // Log the error if necessary
            // Redirect to the error handler with the specific error code
            return RedirectToAction("HandleError", "Error", new { code = (int)ex.StatusCode.Value });
        }
        catch (Exception)
        {
            // Log the error
            // Redirect to a generic server error handler (500)
            return RedirectToAction("HandleError", "Error", new { code = 500 });
        }
    }   
}
