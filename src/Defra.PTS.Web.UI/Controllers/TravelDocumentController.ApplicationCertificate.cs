using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}
