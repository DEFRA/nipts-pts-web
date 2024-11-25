using Defra.PTS.Web.Application.Exceptions;
using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.Controllers;

[Authorize]
public partial class TravelDocumentController : BaseTravelDocumentController
{
    [HttpGet]
    public async Task<IActionResult> ApplicationCertificate(Guid id)
    {
        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        var response = await _mediator.Send(new GetApplicationCertificateQueryRequest(id));
        
        var userId = CurrentUserId();
        if (!response.ApplicationCertificate.UserId.Equals(userId))
        {
            return RedirectToAction("HandleError", "Error", new { code = 404 });
        }

        if (!response.ApplicationCertificate.IsApproved)
        {
            return RedirectToAction(nameof(ApplicationDetails), new { id });
        }
        return View(response.ApplicationCertificate);
    }

    [ExcludeFromCodeCoverage]
    [HttpGet]
    public async Task<IActionResult> DownloadCertificatePdf(Guid id, string referenceNumber)
    {
        var userId = CurrentUserId();

        var response = await _mediator.Send(new GenerateCertificatePdfRequest(id, userId));
        if (response == null)
        {
            return new NotFoundObjectResult("Unable to download the PDF");
        }

        var fileName = ApplicationHelper.BuildPdfDownloadFilename(referenceNumber, PdfType.Certificate);

        return File(response.Content, response.MimeType, fileName);
    }
}
