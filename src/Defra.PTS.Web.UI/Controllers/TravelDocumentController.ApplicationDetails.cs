﻿using Defra.PTS.Web.Application.Features.Certificates.Commands;
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
    public async Task<IActionResult> ApplicationDetails(Guid id)
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

    [ExcludeFromCodeCoverage]
    [HttpGet]
    public async Task<IActionResult> DownloadApplicationDetailsPdf(Guid id)
    {
        var userId = CurrentUserId();

        var response = await _mediator.Send(new GenerateApplicationPdfRequest(id, userId));
        if (response == null)
        {
            return new NotFoundObjectResult("Unable to download the PDF");
        }

        var fileName = ApplicationHelper.BuildPdfDownloadFilename(id, PdfType.Application);

        return File(response.Content, response.MimeType, fileName);
    }
}
