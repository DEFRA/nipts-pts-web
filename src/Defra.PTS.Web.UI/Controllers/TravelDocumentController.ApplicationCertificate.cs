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
    public async Task<IActionResult> ApplicationCertificate()
    {
        var applicationId = new Guid(HttpContext.Session.GetString("ApplicationId"));
        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        var response = await _mediator.Send(new GetApplicationCertificateQueryRequest(applicationId));
        if (!response.ApplicationCertificate.IsApproved)
        {
            return RedirectToAction(nameof(ApplicationDetails), new { applicationId });
        }

        return View(response.ApplicationCertificate);
    }

    [ExcludeFromCodeCoverage]
    [HttpGet]
    public async Task<IActionResult> DownloadCertificatePdf()
    {
        var id = new Guid(HttpContext.Session.GetString("ApplicationId"));

        _logger.LogInformation($"Downloading certificate PDF for {id}");

        var response = await _mediator.Send(new GenerateCertificatePdfRequest(id));
        _logger.LogInformation($"Certificate Name: {response.Name}, FileLength: {response.Content.Length}, MimeType: {response.MimeType}");

        var fileName = ApplicationHelper.BuildPdfDownloadFilename(id, PdfType.Certificate);
        _logger.LogInformation($"Download file name {fileName}");


        return File(response.Content, response.MimeType, fileName);
    }
}
