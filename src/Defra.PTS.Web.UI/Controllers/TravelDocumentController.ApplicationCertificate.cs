using Defra.PTS.Web.Application.Exceptions;
using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Helpers;
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
    [HttpGet("/TravelDocument/ApplicationCertificate/{id}")]
    public async Task<IActionResult> ApplicationCertificate(Guid id)
    {      
        try
        {
            
            var response = await _mediator.Send(new GetApplicationCertificateQueryRequest(id));

            var isInvalidStatus = response.ApplicationCertificate.Status == AppConstants.ApplicationStatus.REVOKED ||
                                    response.ApplicationCertificate.Status == AppConstants.ApplicationStatus.UNSUCCESSFUL;

            var backUrl = isInvalidStatus
                ? WebAppConstants.Pages.TravelDocument.InvalidDocuments
                : WebAppConstants.Pages.TravelDocument.Index;

            SetBackUrl(backUrl);

            var userId = CurrentUserId();
            if (!response.ApplicationCertificate.UserId.Equals(userId))
            {
                return RedirectToAction("HandleError", "Error", new { code = 404 });
            }

            if (!response.ApplicationCertificate.IsApproved && !response.ApplicationCertificate.IsSuspended && !response.ApplicationCertificate.IsRevoked)
            {
                return RedirectToAction(nameof(ApplicationDetails), new { id });
            }
            return View(response.ApplicationCertificate);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [ExcludeFromCodeCoverage]
    [HttpGet("/TravelDocument/DownloadCertificatePdf/{id}/{referenceNumber}")]
    public async Task<IActionResult> DownloadCertificatePdf(Guid id, string referenceNumber)
    {
        try
        {
            var userId = CurrentUserId();

            _logger.LogInformation("DownloadCertificatePdf called with id: {Id}, referenceNumber: {ReferenceNumber}, userId: {UserId}. Attempting to invoke _mediator.Send with GenerateCertificatePdfRequest", id, referenceNumber, userId);
            var response = await _mediator.Send(new GenerateCertificatePdfRequest(id, userId));
            if (response == null)
            {
                _logger.LogWarning("Response from _mediator.send was null, PDF generation failed for id: {Id}, referenceNumber: {ReferenceNumber}, userId: {UserId}", id, referenceNumber, userId);
                return new NotFoundObjectResult("Unable to download the PDF");
            }

            var fileName = ApplicationHelper.BuildPdfDownloadFilename(referenceNumber);
            var fileTitle = "Pet-Travel-Document-" + referenceNumber + ".pdf";

            _logger.LogInformation("PDF generated successfully for id: {Id}, referenceNumber: {ReferenceNumber}, fileName: {FileName}. Attempting to invoke SetFileTitle for download: {FileTitle}", fileTitle, id, referenceNumber, fileName);
            return await SetFileTitle(response, fileName, fileTitle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in DownloadCertificatePdf for id: {Id}, referenceNumber: {ReferenceNumber}, userId: {UserId}", id, referenceNumber, CurrentUserId());
            return HandleException(ex);
        }
    }
}
