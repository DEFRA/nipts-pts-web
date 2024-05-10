using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetApplicationsQueryResponse
{
    public Guid UserId { get; set; }
    public List<ApplicationSummaryDto> Applications { get; set; } = new List<ApplicationSummaryDto>();
}
