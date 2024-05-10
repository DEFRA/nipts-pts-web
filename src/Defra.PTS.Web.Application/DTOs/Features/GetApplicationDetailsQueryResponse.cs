using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetApplicationDetailsQueryResponse
{
    public Guid ApplicationId { get; set; }
    public ApplicationDetailsDto ApplicationDetails { get; set; } = new();
}
