using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.Application.Constants;
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
            var response = await _mediator.Send(new GetApplicationDetailsQueryRequest(id));

            var isInvalidStatus = response.ApplicationDetails.Status == AppConstants.ApplicationStatus.REVOKED ||
                                    response.ApplicationDetails.Status == AppConstants.ApplicationStatus.UNSUCCESSFUL;

            var backUrl = isInvalidStatus
                ? WebAppConstants.Pages.TravelDocument.InvalidDocuments
                : WebAppConstants.Pages.TravelDocument.Index;

            SetBackUrl(backUrl);

            var userId = CurrentUserId();
            if (!response.ApplicationDetails.UserId.Equals(userId))
            {
                return RedirectToAction("HandleError", "Error", new { code = 404 });
            }

            return View(response.ApplicationDetails);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
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
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
