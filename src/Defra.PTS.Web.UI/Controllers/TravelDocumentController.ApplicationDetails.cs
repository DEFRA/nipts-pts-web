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
    public async Task<IActionResult> ApplicationDetails()
    {
        var applicationId =new Guid(HttpContext.Session.GetString("ApplicationId")) ;
        SetBackUrl(WebAppConstants.Pages.TravelDocument.Index);

        var response = await _mediator.Send(new GetApplicationDetailsQueryRequest(applicationId));
        return View(response.ApplicationDetails);
    }

    [ExcludeFromCodeCoverage]
    [HttpGet]
    public async Task<IActionResult> DownloadApplicationDetailsPdf()
    {
        var id = new Guid(HttpContext.Session.GetString("ApplicationId"));

        var response = await _mediator.Send(new GenerateApplicationPdfRequest(id));

        var fileName = ApplicationHelper.BuildPdfDownloadFilename(id, PdfType.Application);

        return File(response.Content, response.MimeType, fileName);
    }
}
